using System.ComponentModel.DataAnnotations;

namespace backendtest.Dtos.ProjectDto
{
    public class UpdateProjectDto
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "GoalAmount must be greater than 0.")]
        public decimal GoalAmount { get; set; }
        
        public int? CategoryId { get; set; }
    }
}