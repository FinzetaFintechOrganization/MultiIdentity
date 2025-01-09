using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
	private readonly UserManager<ApplicationUser> _userManager;

	public UserService(UserManager<ApplicationUser> userManager)
	{
		_userManager = userManager;
	}

	public async Task<UserDTO> GetUserByIdAsync(Guid userId)
	{
		var user = await _userManager.Users
			.FirstOrDefaultAsync(u => u.Id == userId);

		if (user == null)
		{
			throw new KeyNotFoundException($"User with ID {userId} not found.");
		}

		return new UserDTO
		{
			Id = user.Id,
			UserName = user.UserName,
			Email = user.Email,
			CompanyId = user.CompanyId
		};
	}

	public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
	{
		return await _userManager.Users
			.Select(u => new UserDTO
			{
				Id = u.Id,
				UserName = u.UserName,
				Email = u.Email,
				CompanyId = u.CompanyId
			})
			.ToListAsync();
	}

	public async Task<bool> CreateUserAsync(CreateUserDTO dto)
	{
		// Kullanıcıyı oluştur
		var user = new ApplicationUser
		{
			UserName = dto.UserName,
			Email = dto.Email,
			CompanyId = dto.CompanyId
		};

		var result = await _userManager.CreateAsync(user, dto.Password);
		if (!result.Succeeded)
		{
			throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
		}

		return result.Succeeded;
	}

	public async Task<bool> DeleteUserAsync(Guid userId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
		{
			throw new KeyNotFoundException($"User with ID {userId} not found.");
		}

		var result = await _userManager.DeleteAsync(user);
		return result.Succeeded;
	}

	public async Task<bool> AssignRoleToUserAsync(Guid userId, Guid roleId)
	{
		var user = await _userManager.FindByIdAsync(userId.ToString());
		if (user == null)
		{
			throw new KeyNotFoundException($"User with ID {userId} not found.");
		}

		var role = await _userManager.GetRolesAsync(user);
		if (role == null || !role.Any())
		{
			throw new InvalidOperationException($"Role with ID {roleId} not found.");
		}

		var result = await _userManager.AddToRoleAsync(user, role.First());
		if (!result.Succeeded)
		{
			throw new InvalidOperationException($"Failed to assign role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
		}

		return result.Succeeded;
	}
}
