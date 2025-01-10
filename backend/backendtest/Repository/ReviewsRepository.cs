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
    
    public async Task<Review> AddAsync(ReviewDto reviewDto, string userId)
    {
        var guidUserId = Guid.Parse(userId);

        // Проверяем, существует ли проект
        var project = await _context.Projects.FindAsync(reviewDto.ProjectId);
        if (project == null)
            throw new ArgumentException($"Project with ID {reviewDto.ProjectId} does not exist.");

        // Проверяем, существует ли пользователь
        var user = await _context.Users.FindAsync(guidUserId);
        if (user == null)
            throw new ArgumentException($"User with ID {guidUserId} does not exist.");

        // Создаем новый отзыв
        var newReview = new Review
        {
            ProjectId = reviewDto.ProjectId,
            Rating = reviewDto.Rating,
            Comment = reviewDto.Comment,
            UserId = guidUserId,
            Project = project,
            User = user
        };

        await _context.Reviews.AddAsync(newReview);
        await _context.SaveChangesAsync();

        // Обновляем средний рейтинг проекта
        await UpdateProjectAverageRatingAsync(newReview.ProjectId);

        return newReview;
    }

    
    public async Task<bool> UpdateAsync(int id, string userId, UpdateReviewDto updateReviewDto)
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
        return true;
    }

    public async Task<bool> DeleteAsync(int id, string userId)
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
        return true;
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


    public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByProjectIdAsync(int projectId)
    {
        return await _context.Reviews
            .Where(r => r.ProjectId == projectId)
            .Select(r => new ReviewResponseDto
            {
                Id = r.Id,
                ProjectId = r.ProjectId,
                Rating = r.Rating,
                Comment = r.Comment,
                UserName = r.User.UserName, // Предполагается, что в User есть поле Name
                ProjectName = r.Project.Title // Предполагается, что в Project есть поле Name
            })
            .ToListAsync();
    }

}
