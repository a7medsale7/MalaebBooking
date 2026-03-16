// 5. StadiumImagesController.cs
using MalaebBooking.Application.Contracts.Stadiums;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/stadiums/{stadiumId:int}/images")]
[ApiController]
[Authorize]
public class StadiumImagesController(IStadiumImageService stadiumImageService) : ControllerBase
{
    private readonly IStadiumImageService _stadiumImageService = stadiumImageService;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetByStadium(int stadiumId, CancellationToken cancellationToken)
    {
        var result = await _stadiumImageService.GetImagesByStadiumAsync(stadiumId, cancellationToken);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("{imageId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int stadiumId, int imageId, CancellationToken cancellationToken)
    {
        var result = await _stadiumImageService.GetImageByIdAsync(imageId, cancellationToken);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.StadiumImages_Upload)]
    public async Task<IActionResult> Upload(int stadiumId, [FromForm] UploadStadiumImageRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null) return Unauthorized();

        var result = await _stadiumImageService.UploadImageAsync(stadiumId, request, currentUserId, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpPut("{imageId:int}")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.StadiumImages_Update)]
    public async Task<IActionResult> Update(int stadiumId, int imageId, [FromBody] UpdateStadiumImageRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null) return Unauthorized();

        var result = await _stadiumImageService.UpdateImageAsync(imageId, request, currentUserId, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);

        var updated = await _stadiumImageService.GetImageByIdAsync(imageId, cancellationToken);
        return Ok(updated.Value);
    }

    [HttpDelete("{imageId:int}")]
    [Authorize(Roles = $"{DefaultRoles.Admin},{DefaultRoles.Owner}", Policy = Permissions.StadiumImages_Delete)]
    public async Task<IActionResult> Delete(int stadiumId, int imageId, CancellationToken cancellationToken)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (currentUserId is null) return Unauthorized();

        var result = await _stadiumImageService.DeleteImageAsync(imageId, currentUserId, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : NoContent();
    }
}
