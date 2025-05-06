using System.ComponentModel.DataAnnotations.Schema;
namespace backendtest.Models;

public class Project 
{
    public int Id { get; set; } 
    public string Title { get; set; } = string.Empty;
    [Column(TypeName = "text")]
    public string Description { get; set; } = string.Empty; 
    [Column(TypeName = "decimal(18,2)")]
    public decimal GoalAmount { get; set; } 
    [Column(TypeName = "decimal(18,2)")]
    public decimal CollectedAmount { get; set; } = 0; 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public Status Status { get; set; } = Status.Pending; 
    public Guid UserId { get; set; }
    public User User { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Update> Updates { get; set; } = new List<Update>();
    public ICollection<Media> MediaFiles { get; set; } = new List<Media>();
    public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    public float AverageRating { get; set; } = 0;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    
}

