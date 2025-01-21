using System.Security.Claims;
using backendtest.Dtos.UpdateDto;
using backendtest.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backendtest.Controllers;

[Route("api/update")]
[ApiController]
public class UpdateController : ControllerBase
{
    private readonly IUpdateRepository _updateRepository;

    public UpdateController(IUpdateRepository updateRepository)
    {
        _updateRepository = updateRepository;
    }

    [HttpPost("create-update")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> CreateUpdate([FromBody] UpdateDto updateDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var result = await _updateRepository.CreateUpdateAsync(updateDto, userId);
        if (result == null) return Forbid("You do not have permission to create updates for this project.");

        return Ok(result);
    }

    [HttpGet("get-updates")]
    public async Task<IActionResult> GetUpdatesByProjectId(int projectId)
    {
        try
        {
            var updates = await _updateRepository.GetUpdatesByProjectIdAsync(projectId);
            return Ok(updates);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> DeleteUpdate(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        try
        {
            var success = await _updateRepository.DeleteUpdateAsync(id, userId);
            if (!success) return NotFound("Update not found.");

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize("UserPolicy")]
    public async Task<IActionResult> UpdateUpdate(int id, [FromBody] UpdateDto updateDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        try
        {
            var updatedUpdate = await _updateRepository.UpdateUpdateAsync(id, updateDto, userId);
            return Ok(updatedUpdate);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    //АДМИНСКИЕ МЕТОДЫ
    [HttpPost("create-update-for-admin")]
    [Authorize("AdminPolicy")]
    public async Task<IActionResult> CreateUpdateForAdmin([FromBody] UpdateDto updateDto)
    {
        var result = await _updateRepository.CreateUpdateForAdminAsync(updateDto);
        if (result == null) return Forbid("You do not have permission to create updates for this project.");
        return Ok(result);
    }

    [HttpDelete("delete-update-for-admin")]
    [Authorize("AdminPolicy")]
    public async Task<IActionResult> DeleteUpdateForAdmin(int id)
    {
        try
        {
            var success = await _updateRepository.DeleteUpdateForAdminAsync(id);
            if (!success) return NotFound("Update not found.");

            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpPut("update-update-for-admin")]
    [Authorize("AdminPolicy")]
    public async Task<IActionResult> UpdateUpdateForAdmin(int id, [FromBody] UpdateDto updateDto)
    {
        try
        {
            var updatedUpdate = await _updateRepository.UpdateUpdateForAdminAsync(id, updateDto);
            return Ok(updatedUpdate);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
