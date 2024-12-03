public class UpdatePermissionDTO
{
	public Guid Id { get; set; }
	public string ModuleName { get; set; }
	public string Action { get; set; }
	public string Category { get; set; }
	public string Description { get; set; }
}
