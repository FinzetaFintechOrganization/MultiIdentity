using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
	public void Configure(EntityTypeBuilder<ApplicationRole> builder)
	{
		builder.HasKey(r => r.Id);

		builder.HasMany(r => r.UserRoles)
			.WithOne(ur => ur.Role)
			.HasForeignKey(ur => ur.RoleId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(r => r.PermissionRoles)
			.WithOne(pr => pr.Role)
			.HasForeignKey(pr => pr.RoleId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
