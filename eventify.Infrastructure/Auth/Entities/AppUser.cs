using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser<Guid>
{
    public Guid MemberId { get; set; }
}
