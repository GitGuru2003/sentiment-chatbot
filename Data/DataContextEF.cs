using chatbot.Models;
using Microsoft.EntityFrameworkCore;

namespace chatbot.Data
{
  public class DataContextEF : DbContext
  {
    private readonly IConfiguration _config;

    public DataContextEF(IConfiguration config)
    {
      _config = config;
    }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"), (options) => { options.EnableRetryOnFailure(); });
      }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("Chatbot");
      modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId);
      modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages").HasKey(c => c.MessageId);
      modelBuilder.Entity<ChatMessage>().HasOne(cm => cm.User).WithMany(u => u.ChatMessages).HasForeignKey(cm => cm.UserId).OnDelete(DeleteBehavior.Cascade);
    }




  }
}