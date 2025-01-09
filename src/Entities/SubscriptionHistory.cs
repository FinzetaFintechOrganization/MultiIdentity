public class SubscriptionHistory
{
	public Guid Id { get; set; }
	public Guid CompanyId { get; set; }
	public Company Company { get; set; }
	public DateTime StartDate { get; set; } 
	public DateTime EndDate { get; set; } 
	public decimal Price { get; set; } 
	public bool IsTrial { get; set; }
}
