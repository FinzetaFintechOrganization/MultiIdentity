using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Configuration class for the Company entity.
/// </summary>
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
	/// <summary>
	/// Configures the Company entity.
	/// </summary>
	/// <param name="builder">The builder to be used for configuring the entity.</param>
	public void Configure(EntityTypeBuilder<Company> builder)
	{
		// Set the primary key
		builder.HasKey(c => c.Id);

		// Configure the Name property
		builder.Property(c => c.Name)
			.IsRequired() // Name is required
			.HasMaxLength(255); // Maximum length of 255 characters

		// Configure the PhoneNumber property
		builder.Property(c => c.PhoneNumber)
			.HasMaxLength(13); // Maximum length of 13 characters

		builder.HasIndex(c => c.PhoneNumber)
			.IsUnique() // Add a unique constraint
			.HasDatabaseName("IX_Company_PhoneNumber");

		// Configure the VKN property
		builder.Property(c => c.VKN)
			.HasMaxLength(11); // Maximum length of 11 characters

		builder.HasIndex(c => c.VKN)
			.IsUnique() // Add a unique constraint
			.HasDatabaseName("IX_Company_VKN"); // Optional: Name of the unique index

		// Configure the CreatedDate property
		builder.Property(c => c.CreatedDate)
			.IsRequired(); // CreatedDate is required

		// Configure the IsTrial property
		builder.Property(c => c.IsTrial)
			.IsRequired(); // IsTrial is required

		// Configure the TrialEndDate property
		builder.Property(c => c.TrialEndDate)
			.IsRequired(); // TrialEndDate is required

		// Configure the SubscriptionEndDate property
		builder.Property(c => c.SubscriptionEndDate)
			.IsRequired(false); // SubscriptionEndDate is optional

		// Configure the relationship between Company and ApplicationUser
		builder.HasMany(c => c.Users)
			.WithOne(u => u.Company) // Navigation property in ApplicationUser
			.HasForeignKey(u => u.CompanyId) // Foreign key property in ApplicationUser
			.OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

		// Configure the relationship between Company and ApplicationRole
		builder.HasMany(c => c.Roles)
			.WithOne(r => r.Company) // Navigation property in ApplicationRole
			.HasForeignKey(r => r.CompanyId) // Foreign key property in ApplicationRole
			.OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

		// Configure the relationship between Company and SubscriptionHistory
		builder.HasMany(c => c.SubscriptionHistories)
			.WithOne(sh => sh.Company) // Navigation property in SubscriptionHistory
			.HasForeignKey(sh => sh.CompanyId) // Foreign key property in SubscriptionHistory
			.OnDelete(DeleteBehavior.Cascade); // Allow cascade delete
	}
}
