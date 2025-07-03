namespace chatbot.Dtos
{
  public partial class UserForRegistrationDto
  {
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Gender { get; set; } = "";
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
    public string Role { get; set; } = "User"; // User role will be the default
  }
}