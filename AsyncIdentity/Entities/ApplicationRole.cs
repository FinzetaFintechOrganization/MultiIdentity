using Microsoft.AspNetCore.Identity;
using System.Security;

public class ApplicationRole : IdentityRole<Guid>
{
	public Guid CompanyId { get; set; }
	public Company Company { get; set; }
	public ICollection<PermissionRole> PermissionRoles { get; set; }
	public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}