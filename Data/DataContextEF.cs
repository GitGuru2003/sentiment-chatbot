using chatbot.Models;
using Microsoft.EntityFrameworkCore;

namespace chatbot.Data
{
  public class DataContextEF : DbContext
  {
    // Private VS Protected
    // Private fields or methods are only accessible inside the class they are declared in.
    // Protected fields or methods can be accessible inside the class they are declared in and also in any derived (inherited) class.
    private readonly IConfiguration _config; // Private means this field is only accessible within this class only.
                                             // If you want to access this field in derived classes, you can use protected instead of private.
                                             // readonly means that this field can only be assigned once, either at declaration or in the constructor.
    public DataContextEF(IConfiguration config)
    {
      _config = config; // Assigning the configuration object to the private field.
    }

    public virtual DbSet<User> Users { get; set; } // DbSet is a collection of entities of single type that can be queried from the database.
                                                   // Virtual means that this method can be overriden in the derived or inherited classes.

    public virtual DbSet<ChatMessage> ChatMessages { get; set; } // Virtual is used for polymorphism, dynamic polymorphism. 

    // Override replaces base class method with your custom implementation
    // Protected means that this method can be accessed by this class and any derived (inherited) classes.
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"), (options) => { options.EnableRetryOnFailure(); });
      }

    }

    // void means that this method does not return any value.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("Chatbot");
      modelBuilder.Entity<User>().ToTable("Users").HasKey(u => u.UserId);
      modelBuilder.Entity<ChatMessage>().ToTable("ChatMessages").HasKey(c => c.MessageId);
      modelBuilder.Entity<ChatMessage>().HasOne(cm => cm.User).WithMany(u => u.ChatMessages).HasForeignKey(cm => cm.UserId).OnDelete(DeleteBehavior.Cascade);
    }
  }
}