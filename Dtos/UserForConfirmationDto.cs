namespace chatbot.Dtos
{
  public partial class UserForConfirmationDto
  {
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }

    public UserForConfirmationDto()
    {
      PasswordHash = new byte[0];
      PasswordSalt = new byte[0];
    }
  }
}