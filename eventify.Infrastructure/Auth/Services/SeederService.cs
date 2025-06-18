using eventify.Domain.Entities;
using eventify.Domain.ValueObjects;
using eventify.Infrastructure.Extensions;
using eventify.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System; // Needed for Guid

public static class IdentitySeeder
{
    public static async Task SeedRolesAndAdminUserAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
        var dbContext = serviceProvider.GetRequiredService<AppIdentityDbContext>();
        var eventsDbContext = serviceProvider.GetRequiredService<EventsDbContext>();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        // 🔐 1. Seed roles
        var roles = new[] { "Admin", "User" };
        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
        }

        var adminEmail = configuration["Admin:email"] ?? throw new Exception("Admin email not found in configuration.");
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            // Create domain Member first
            var memberName = Name.Create("Admin");
            var memberSurname = Name.Create("User");
            var memberEmail = Email.Create(adminEmail);
            // Validate Member creation result
            var memberCreationResult = Member.Create(memberName.Value, memberSurname.Value, memberEmail.Value);
            if (memberCreationResult.IsFailure)
                throw new Exception("Member creation failed: " + memberCreationResult.Error);
            var newMember = memberCreationResult.Value;

            eventsDbContext.Members.Add(newMember);
            await eventsDbContext.SaveChangesAsync();

            var newAppUser = new AppUser
            {
                UserName = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                MemberId = newMember.Id,
            };

            var adminPassword = configuration["Admin:Password"] ?? throw new Exception("Admin password not found in configuration.");

            var result = await userManager.CreateAsync(newAppUser, adminPassword);

            if (!result.Succeeded)
                throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            await userManager.AddToRoleAsync(newAppUser, "Admin");
        }
    }
}
