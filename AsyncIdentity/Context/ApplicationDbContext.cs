using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
	public DbSet<Company> Companies { get; set; }
	public DbSet<Permission> Permissions { get; set; }
	public DbSet<PermissionRole> PermissionRoles { get; set; }
	public DbSet<ApplicationUserRole> UserRoles { get; set; }
	public DbSet<SubscriptionHistory> SubscriptionHistories { get; set; }

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Ignore<IdentityUserRole<Guid>>();

		builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
	}
}
