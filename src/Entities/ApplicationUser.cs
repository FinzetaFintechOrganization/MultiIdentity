using Microsoft.AspNetCore.Identity;
using System.Data;

public class ApplicationUser : IdentityUser<Guid>
{
	public Guid CompanyId { get; set; }
	public Company Company { get; set; }
	public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}
