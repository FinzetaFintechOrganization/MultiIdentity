using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
	public void Configure(EntityTypeBuilder<Permission> builder)
	{
		builder.HasKey(p => p.Id);

		builder.Property(p => p.ModuleName)
			.IsRequired()
			.HasMaxLength(255);

		builder.Property(p => p.Action)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(p => p.Category)
			.HasMaxLength(255);

		builder.Property(p => p.Description)
			.HasMaxLength(1000);
	}
}
