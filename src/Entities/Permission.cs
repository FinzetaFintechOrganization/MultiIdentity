﻿public class Permission
{
	public Guid Id { get; set; }
	public string ModuleName { get; set; } 
	public string Action { get; set; }  
	public string Category { get; set; }
	public string Description { get; set; }

	public ICollection<PermissionRole> PermissionRoles { get; set; }
}
