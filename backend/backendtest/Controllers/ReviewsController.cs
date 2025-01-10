using System.Security.Claims;
using backendtest.Dtos.ReviewDto;
using backendtest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;

[Route("api/reviews")]
 [ApiController]
public class ReviewsController : ControllerBase
{
    private readonly IReviewsRepository _reviewsRepo;

    public ReviewsController(IReviewsRepository reviewsRepo)
    {
        _reviewsRepo = reviewsRepo;
    }
    
    [HttpPost("createReview")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized();

        try
        {
            var review = await _reviewsRepo.AddAsync(reviewDto, userId);

            // Формируем ответ
            var response = new ReviewResponseDto
            {
                Id = review.Id,
                ProjectId = review.ProjectId,
                Rating = review.Rating,
                Comment = review.Comment,
                UserName = review.User.UserName, // Предполагается, что в User есть поле Name
                ProjectName = review.Project.Title // Предполагается, что в Project есть поле Name
            };

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    [HttpPut("updateReview")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> UpdateReview( int productId, [FromBody] UpdateReviewDto updateReviewDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userId==null)
            return Unauthorized();
        var updated = await _reviewsRepo.UpdateAsync(productId, userId, updateReviewDto);
        if (!updated)
        {
            return BadRequest("Review update failed.");
        }
        return Ok("Review updated successfully.");
        
    }

    [HttpDelete("deleteReview")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> DeleteReview(int productId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(userId == null)
            return Unauthorized();
        var deleted = await _reviewsRepo.DeleteAsync(productId, userId);
        if (!deleted)
        { 
            return BadRequest("Review deletion failed.");
        }
        return Ok("Review deleted successfully.");        
    }
    
    [HttpGet("one-project-reviews")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> GetReviewsByProjectId(int projectId)
    {
        try
        {
            var reviews = await _reviewsRepo.GetReviewsByProjectIdAsync(projectId);
            if (!reviews.Any())
                return NotFound(new { message = "No reviews found for this project." });

            return Ok(reviews);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching reviews.", details = ex.Message });
        }
    }

}