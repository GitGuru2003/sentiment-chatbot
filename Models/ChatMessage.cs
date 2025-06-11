namespace chatbot.Models
{
  public partial class ChatMessage
  {
    public int MessageId { get; set; }
    public int UserId { get; set; }
    public string MessageText { get; set; } = "";
    public string Sentiment { get; set; } = "";
    public float SentimentScore { get; set; }
    public string BotResponse { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User? User { get; set; }
  }
}