using backendtest.Dtos.ReviewDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IReviewsRepository
{
    Task AddAsync(ReviewDto reviewDto, string userId);
    Task UpdateAsync(int id, string userId, UpdateReviewDto updateReviewDto);
    Task DeleteAsync(int id, string userId);
    Task UpdateProjectAverageRatingAsync(int projectId);
    Task<IEnumerable<Review>> GetReviewByProjectIdAsync(int projectId);
}