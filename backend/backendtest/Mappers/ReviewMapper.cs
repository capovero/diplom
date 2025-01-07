using backendtest.Models;
using backendtest.Dtos.ReviewDto;
using System.Linq;

namespace backendtest.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewDto ToProjectResponseDto(this Review review)
        {
            return new ReviewDto
            {
               ProjectId = review.ProjectId,
               Rating = review.Rating,
               Comment = review.Comment,
            };
        }
        
    }
}