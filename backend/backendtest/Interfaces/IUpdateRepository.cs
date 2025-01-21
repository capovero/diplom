using backendtest.Dtos.UpdateDto;

namespace backendtest.Interfaces;

public interface IUpdateRepository
{
    Task<UpdateResponseDto?> CreateUpdateAsync(UpdateDto updateDto, string userId);
    Task<List<UpdateResponseDto>> GetUpdatesByProjectIdAsync(int projectId);
    Task<bool> DeleteUpdateAsync(int updateId, string userId);
    Task<UpdateResponseDto> UpdateUpdateAsync(int updateId, UpdateDto updateDto, string userId);
    
    Task<UpdateResponseDto?> CreateUpdateForAdminAsync(UpdateDto updateDto);
    Task<bool> DeleteUpdateForAdminAsync(int updateId);
    Task<UpdateResponseDto> UpdateUpdateForAdminAsync(int updateId, UpdateDto updateDto);

}