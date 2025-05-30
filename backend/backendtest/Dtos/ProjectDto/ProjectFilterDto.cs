using backendtest.Models;

public class ProjectFilterDto
{
    public string? Title { get; set; }
    public int? CategoryId { get; set; }
    public Status? Status { get; set; }
}
