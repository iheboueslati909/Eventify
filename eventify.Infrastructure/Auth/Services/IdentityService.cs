using eventify.Application.Common; 
using eventify.Application.Repositories;
using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.SharedKernel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace eventify.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMemberRepository _memberRepository;
    private readonly IConfiguration _configuration;

    public IdentityService(
        UserManager<AppUser> userManager,
        IMemberRepository memberRepository,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _memberRepository = memberRepository;
        _configuration = configuration;
    }

    public async Task<Result<Guid>> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        // Create domain entities/value objects
        var firstNameResult = Name.Create(firstName);
        var lastNameResult = Name.Create(lastName);
        var emailResult = Email.Create(email);
        var passwordResult = Password.Create(password);

        if (firstNameResult.IsFailure) return Result.Failure<Guid>(firstNameResult.Error);
        if (lastNameResult.IsFailure) return Result.Failure<Guid>(lastNameResult.Error);
        if (emailResult.IsFailure) return Result.Failure<Guid>(emailResult.Error);
        if (passwordResult.IsFailure) return Result.Failure<Guid>(passwordResult.Error);

        // Create member
        var memberResult = Member.Create(firstNameResult.Value, lastNameResult.Value, emailResult.Value);
        if (memberResult.IsFailure) return Result.Failure<Guid>(memberResult.Error);

        var member = memberResult.Value;
        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();

        // Create identity user
        var user = new AppUser
        {
            Email = email,
            UserName = email,
            MemberId = member.Id
        };

        var identityResult = await _userManager.CreateAsync(user, password);
        if (!identityResult.Succeeded)
        {
            return Result.Failure<Guid>(string.Join(", ", identityResult.Errors.Select(e => e.Description)));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            return Result.Failure<Guid>(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        return Result.Success(member.Id);
    }

    public async Task<Result<string>> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return Result.Failure<string>("Invalid credentials");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);
        if (!isPasswordValid) return Result.Failure<string>("Invalid credentials");

        var token = await GenerateJwtToken(user);
        return Result.Success(token);
    }

    private async Task<string> GenerateJwtToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? 
            throw new InvalidOperationException("JWT:Key not configured")));

        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var appId = _configuration["AppId"] ?? 
            throw new InvalidOperationException("AppSettings:AppId not configured");

        var roles = await _userManager.GetRolesAsync(user);
        if (roles == null || !roles.Any())
        {
            throw new InvalidOperationException("User has no roles assigned.");
        }

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new Claim("MemberId", user.MemberId.ToString()),
            new Claim("appId", appId)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


        var securityToken = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpiryMinutes"])),
            signingCredentials: signingCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }
}
