/// <summary>
/// DTO for registering a new user and company.
/// </summary>
public class RegisterRequest
{

	/// <summary>
	/// User's email address.
	/// </summary>
	public string Email { get; set; }

	/// <summary>
	/// User's password.
	/// </summary>
	public string Password { get; set; }

	/// <summary>
	/// Company's name.
	/// </summary>
	public string CompanyName { get; set; }

	/// <summary>
	/// Company's phone number.
	/// </summary>
	public string PhoneNumber { get; set; }

	/// <summary>
	/// Company's tax identification number.
	/// </summary>
	public string VKN { get; set; }
}
