using backendtest.Models;

public class ProjectFilterDto
{
    public string? Title { get; set; }
    public int? CategoryId { get; set; } = 1;
    public Status? Status { get; set; }
}
