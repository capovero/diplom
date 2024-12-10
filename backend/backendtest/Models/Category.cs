namespace backendtest.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    
    // Одна категория может быть привязана к нескольким проектам
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
