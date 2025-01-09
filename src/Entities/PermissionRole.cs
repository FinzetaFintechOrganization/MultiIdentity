using System.Data;

public class PermissionRole
{
	public Guid RoleId { get; set; }
	public ApplicationRole Role { get; set; }

	public Guid PermissionId { get; set; }
	public Permission Permission { get; set; }
}
