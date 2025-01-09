using Microsoft.EntityFrameworkCore;

public class PermissionService : IPermissionService
{
	private readonly ApplicationDbContext _context;

	public PermissionService(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<bool> AssignPermissionToRoleAsync(AssignPermissionDTO dto)
	{
		var roleExists = await _context.Roles.AnyAsync(r => r.Id == dto.RoleId);
		var permissionExists = await _context.Permissions.AnyAsync(p => p.Id == dto.PermissionId);

		if (!roleExists)
			throw new KeyNotFoundException($"Role with ID {dto.RoleId} not found.");

		if (!permissionExists)
			throw new KeyNotFoundException($"Permission with ID {dto.PermissionId} not found.");

		var alreadyAssigned = await _context.PermissionRoles
			.AnyAsync(pr => pr.RoleId == dto.RoleId && pr.PermissionId == dto.PermissionId);

		if (alreadyAssigned)
			throw new InvalidOperationException("Permission is already assigned to the role.");

		var permissionRole = new PermissionRole
		{
			RoleId = dto.RoleId,
			PermissionId = dto.PermissionId
		};

		_context.PermissionRoles.Add(permissionRole);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<IEnumerable<PermissionDTO>> GetPermissionsByRoleAsync(Guid roleId)
	{
		var roleExists = await _context.Roles.AnyAsync(r => r.Id == roleId);
		if (!roleExists)
			throw new KeyNotFoundException($"Role with ID {roleId} not found.");

		return await _context.PermissionRoles
			.Where(pr => pr.RoleId == roleId)
			.Select(pr => new PermissionDTO
			{
				Id = pr.Permission.Id,
				ModuleName = pr.Permission.ModuleName,
				Action = pr.Permission.Action,
				Category = pr.Permission.Category,
				Description = pr.Permission.Description
			})
			.ToListAsync();
	}

	public async Task<PermissionDTO> GetPermissionByIdAsync(Guid id)
	{
		var permission = await _context.Permissions.FindAsync(id);

		if (permission == null)
			throw new KeyNotFoundException($"Permission with ID {id} not found.");

		return new PermissionDTO
		{
			Id = permission.Id,
			ModuleName = permission.ModuleName,
			Action = permission.Action,
			Category = permission.Category,
			Description = permission.Description
		};
	}

	public async Task<IEnumerable<PermissionDTO>> GetAllPermissionsAsync()
	{
		return await _context.Permissions
			.Select(p => new PermissionDTO
			{
				Id = p.Id,
				ModuleName = p.ModuleName,
				Action = p.Action,
				Category = p.Category,
				Description = p.Description
			})
			.ToListAsync();
	}

	public async Task<bool> CreatePermissionAsync(CreatePermissionDTO dto)
	{
		var permission = new Permission
		{
			Id = Guid.NewGuid(),
			ModuleName = dto.ModuleName,
			Action = dto.Action,
			Category = dto.Category,
			Description = dto.Description
		};

		_context.Permissions.Add(permission);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> UpdatePermissionAsync(UpdatePermissionDTO dto)
	{
		var permission = await _context.Permissions.FindAsync(dto.Id);

		if (permission == null)
			throw new KeyNotFoundException($"Permission with ID {dto.Id} not found.");

		permission.ModuleName = dto.ModuleName;
		permission.Action = dto.Action;
		permission.Category = dto.Category;
		permission.Description = dto.Description;

		_context.Permissions.Update(permission);
		return await _context.SaveChangesAsync() > 0;
	}

	public async Task<bool> DeletePermissionAsync(Guid id)
	{
		var permission = await _context.Permissions.FindAsync(id);

		if (permission == null)
			throw new KeyNotFoundException($"Permission with ID {id} not found.");

		_context.Permissions.Remove(permission);
		return await _context.SaveChangesAsync() > 0;
	}
}
