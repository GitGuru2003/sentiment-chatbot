namespace chatbot.Models
{
  public partial class User
  {
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Gender { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<ChatMessage> ChatMessages { get; set; }

    public User()
    {
      FirstName = "";
      LastName = "";
      Email = "";
      Gender = "";
      PasswordHash = new byte[0];
      PasswordSalt = new byte[0];
      CreatedAt = DateTime.UtcNow;
      ChatMessages = new List<ChatMessage>();
    }

  }
}