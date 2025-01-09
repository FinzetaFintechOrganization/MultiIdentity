using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class RoleService : IRoleService
{
	private readonly RoleManager<ApplicationRole> _roleManager;

	public RoleService(RoleManager<ApplicationRole> roleManager)
	{
		_roleManager = roleManager;
	}

	public async Task<RoleDTO> GetRoleByIdAsync(Guid roleId)
	{
		var role = await _roleManager.Roles
			.FirstOrDefaultAsync(r => r.Id == roleId);

		if (role == null)
		{
			throw new KeyNotFoundException($"Role with ID {roleId} not found.");
		}

		return new RoleDTO
		{
			Id = role.Id,
			Name = role.Name,
			CompanyId = role.CompanyId
		};
	}

	public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
	{
		return await _roleManager.Roles
			.Select(r => new RoleDTO
			{
				Id = r.Id,
				Name = r.Name,
				CompanyId = r.CompanyId
			})
			.ToListAsync();
	}

	public async Task<bool> CreateRoleAsync(CreateRoleDTO dto)
	{
		var existingRole = await _roleManager.FindByNameAsync(dto.Name);
		if (existingRole != null)
		{
			throw new InvalidOperationException($"Role with name '{dto.Name}' already exists.");
		}

		var role = new ApplicationRole
		{
			Name = dto.Name,
			CompanyId = dto.CompanyId
		};

		var result = await _roleManager.CreateAsync(role);
		return result.Succeeded;
	}

	public async Task<bool> DeleteRoleAsync(Guid roleId)
	{
		var role = await _roleManager.FindByIdAsync(roleId.ToString());
		if (role == null)
		{
			throw new KeyNotFoundException($"Role with ID {roleId} not found.");
		}

		var result = await _roleManager.DeleteAsync(role);
		return result.Succeeded;
	}
}
