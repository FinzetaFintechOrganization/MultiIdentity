public class SubscriptionDTO
{
	public Guid CompanyId { get; set; }
	public DateTime EndDate { get; set; }
	public decimal Price { get; set; }
	public bool IsTrial { get; set; }
}
