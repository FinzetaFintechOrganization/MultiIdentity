using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SubscriptionHistoryConfiguration : IEntityTypeConfiguration<SubscriptionHistory>
{
	public void Configure(EntityTypeBuilder<SubscriptionHistory> builder)
	{
		builder.HasKey(sh => sh.Id);

		builder.HasOne(sh => sh.Company)
			.WithMany(c => c.SubscriptionHistories)
			.HasForeignKey(sh => sh.CompanyId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
