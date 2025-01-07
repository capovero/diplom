using backendtest.Data;
using backendtest.Dtos.ReviewDto;
using backendtest.Interfaces;
using backendtest.Models;
using Microsoft.EntityFrameworkCore;

public class ReviewsRepository : IReviewsRepository
{
    private readonly ApplicationContext _context;

    public ReviewsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task AddAsync(ReviewDto reviewDto, string userId)
    {
        var guidUserId = Guid.Parse(userId);
        var newReview = new Review
        {
            ProjectId = reviewDto.ProjectId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            UserId = guidUserId
        };
        await _context.Reviews.AddAsync(newReview);
        await _context.SaveChangesAsync();
        await UpdateProjectAverageRatingAsync(newReview.ProjectId);
    }
    
    public async Task UpdateAsync(int id, string userId, UpdateReviewDto updateReviewDto)
    {
        var guidUserId = Guid.Parse(userId);
        var review = await _context.Reviews.FindAsync(id);
        
        if (review == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }
        
        if (review.UserId != guidUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this review.");
        }

        review.Rating = updateReviewDto.Rating;
        review.Comment = updateReviewDto.Comment;

        await _context.SaveChangesAsync();
        await UpdateProjectAverageRatingAsync(review.ProjectId); // Пересчёт рейтинга после обновления
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var guidUserId = Guid.Parse(userId);
        var review = await _context.Reviews.FindAsync(id);

        if (review == null)
        {
            throw new KeyNotFoundException("Review not found.");
        }

        if (review.UserId != guidUserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this review.");
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
        await UpdateProjectAverageRatingAsync(review.ProjectId); // Пересчёт рейтинга после удаления
    }

    public async Task UpdateProjectAverageRatingAsync(int projectId)
    {
        var project = await _context.Projects
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        if (project == null)
        {
            throw new KeyNotFoundException("Project not found.");
        }

        var averageRating = project.Reviews.Any() 
            ? (float)project.Reviews.Average(r => r.Rating) 
            : 0;

        project.AverageRating = averageRating;
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<Review>> GetReviewByProjectIdAsync(int projectId)
    {
        return await _context.Reviews
            .Where(r => r.ProjectId == projectId)
            .ToListAsync();
    }
}
