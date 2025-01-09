public interface IPermissionService
{
	Task<bool> AssignPermissionToRoleAsync(AssignPermissionDTO dto);
	Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleAsync(Guid roleId);
	Task<PermissionDTO> GetPermissionByIdAsync(Guid id);
	Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync();
	Task<bool> CreatePermissionAsync(CreatePermissionDTO dto);
	Task<bool> UpdatePermissionAsync(UpdatePermissionDTO dto);
	Task<bool> DeletePermissionAsync(Guid id);
}
