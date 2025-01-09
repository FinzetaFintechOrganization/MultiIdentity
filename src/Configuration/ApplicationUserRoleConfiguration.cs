﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
{
	public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
	{
		builder.HasKey(ur => new { ur.UserId, ur.RoleId });

		builder.HasOne(ur => ur.User)
			.WithMany(u => u.UserRoles)
			.HasForeignKey(ur => ur.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(ur => ur.Role)
			.WithMany(r => r.UserRoles)
			.HasForeignKey(ur => ur.RoleId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
