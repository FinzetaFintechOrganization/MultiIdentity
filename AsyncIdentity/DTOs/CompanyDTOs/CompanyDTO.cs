public class CompanyDTO
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string PhoneNumber { get; set; }
	public string VKN { get; set; }
	public DateTime CreatedDate { get; set; }
	public DateTime? SubscriptionEndDate { get; set; }
	public bool IsTrial { get; set; }
	public DateTime TrialEndDate { get; set; }
}
