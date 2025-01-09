public interface IRoleService
{
	Task<RoleDTO> GetRoleByIdAsync(Guid roleId);
	Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
	Task<bool> CreateRoleAsync(CreateRoleDTO dto);
	Task<bool> DeleteRoleAsync(Guid roleId);
}
