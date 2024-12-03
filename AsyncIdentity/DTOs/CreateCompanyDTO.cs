public class CreateCompanyDTO
{
	public string Name { get; set; }
	public string PhoneNumber { get; set; }
	public string VKN { get; set; }
	public bool IsTrial { get; set; } = true;
	public DateTime TrialEndDate { get; set; } = DateTime.UtcNow.AddMonths(1); // 1 aylık deneme süresi
}
