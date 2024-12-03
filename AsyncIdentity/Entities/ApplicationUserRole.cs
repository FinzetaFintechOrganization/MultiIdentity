using Microsoft.AspNetCore.Identity;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
	public ApplicationUser User { get; set; }
	public ApplicationRole Role { get; set; }
}
