using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissionRoleConfiguration : IEntityTypeConfiguration<PermissionRole>
{
	public void Configure(EntityTypeBuilder<PermissionRole> builder)
	{
		builder.HasKey(pr => new { pr.RoleId, pr.PermissionId });

		builder.HasOne(pr => pr.Role)
			.WithMany(r => r.PermissionRoles)
			.HasForeignKey(pr => pr.RoleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(pr => pr.Permission)
			.WithMany(p => p.PermissionRoles)
			.HasForeignKey(pr => pr.PermissionId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
