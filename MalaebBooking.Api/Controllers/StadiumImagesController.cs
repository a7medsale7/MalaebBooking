// MalaebBooking.Api/Controllers/StadiumImagesController.cs

using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/stadiums/{stadiumId:int}/images")]
[Authorize]
[ApiController]
public class StadiumImagesController(IStadiumImageService stadiumImageService) : ControllerBase
{
    private readonly IStadiumImageService _stadiumImageService = stadiumImageService;

    // ================== GET ALL IMAGES FOR A STADIUM ==================
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetByStadium(int stadiumId, CancellationToken cancellationToken)
    {
        var result = await _stadiumImageService.GetImagesByStadiumAsync(stadiumId, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    // ================== GET SINGLE IMAGE BY ID ==================
    [HttpGet("{imageId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int stadiumId, int imageId, CancellationToken cancellationToken)
    {
        var result = await _stadiumImageService.GetImageByIdAsync(imageId, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    // ================== UPLOAD IMAGE ==================
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(int stadiumId, [FromForm] UploadStadiumImageRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null)
            return Unauthorized();

        var result = await _stadiumImageService.UploadImageAsync(stadiumId, request, currentUserId, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    // ================== UPDATE IMAGE (IsPrimary / DisplayOrder) ==================
    [HttpPut("{imageId:int}")]
    public async Task<IActionResult> Update(int stadiumId, int imageId, [FromBody] UpdateStadiumImageRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null)
            return Unauthorized();

        var result = await _stadiumImageService.UpdateImageAsync(imageId, request, currentUserId, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        var updated = await _stadiumImageService.GetImageByIdAsync(imageId, cancellationToken);
        return Ok(updated.Value);
    }

    // ================== DELETE IMAGE ==================
    [HttpDelete("{imageId:int}")]
    public async Task<IActionResult> Delete(int stadiumId, int imageId, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null)
            return Unauthorized();

        var result = await _stadiumImageService.DeleteImageAsync(imageId, currentUserId, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        return NoContent();
    }
}
