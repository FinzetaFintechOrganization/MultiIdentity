public interface IUserService
{
	Task<UserDTO> GetUserByIdAsync(Guid userId);
	Task<IEnumerable<UserDTO>> GetAllUsersAsync();
	Task<bool> CreateUserAsync(CreateUserDTO dto);
	Task<bool> DeleteUserAsync(Guid userId);
	Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId);
}
