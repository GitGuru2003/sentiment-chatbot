namespace chatbot.Dtos
{
  public partial class UserToAddDto
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }

    public UserToAddDto()
    {
      FirstName = "";
      LastName = "";
      Email = "";
      Gender = "";
    }
  }
}