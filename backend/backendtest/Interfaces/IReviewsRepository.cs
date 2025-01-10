using backendtest.Dtos.ReviewDto;
using backendtest.Models;

namespace backendtest.Interfaces;

public interface IReviewsRepository
{
    Task<Review> AddAsync(ReviewDto reviewDto, string userId);
    Task<bool> UpdateAsync(int id, string userId, UpdateReviewDto updateReviewDto);
    Task<bool> DeleteAsync(int id, string userId);
    Task UpdateProjectAverageRatingAsync(int projectId);
    Task<IEnumerable<ReviewResponseDto>> GetReviewsByProjectIdAsync(int projectId);
}