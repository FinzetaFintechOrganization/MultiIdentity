using System.Data;

public class Company
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string PhoneNumber { get; set; }
	public string VKN { get; set; }
	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
	public DateTime? SubscriptionEndDate { get; set; } 
	public bool IsTrial { get; set; } = true;
	public DateTime TrialEndDate { get; set; } 

	public ICollection<ApplicationUser> Users { get; set; }
	public ICollection<ApplicationRole> Roles { get; set; }
	public ICollection<SubscriptionHistory> SubscriptionHistories { get; set; }

}
