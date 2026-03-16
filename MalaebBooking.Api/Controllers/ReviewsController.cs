// 3. ReviewsController.cs
using MalaebBooking.Application.Contracts.Reviews;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewsController(IReviewService reviewService) : ControllerBase
{
    private readonly IReviewService _reviewService = reviewService;

    [HttpPost]
    [Authorize(Policy = Permissions.Reviews_Create)]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var result = await _reviewService.CreateReviewAsync(request, playerId);
        return result.IsFailure ? BadRequest(result.Error) : Ok(new { Message = "تم إضافة التقييم بنجاح." });
    }

    [HttpPut("{id}")]
    [Authorize(Policy = Permissions.Reviews_Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewRequest request)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var result = await _reviewService.UpdateReviewAsync(id, request, playerId);
        if (result.IsFailure)
        {
            if (result.Error.Code == "Review.NotFound") return NotFound(result.Error);
            if (result.Error.Code == "Review.NotAuthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return Ok(new { Message = "تم تعديل التقييم بنجاح." });
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = Permissions.Reviews_Delete)]
    public async Task<IActionResult> Delete(int id)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var result = await _reviewService.DeleteReviewAsync(id, playerId);
        if (result.IsFailure)
        {
            if (result.Error.Code == "Review.NotFound") return NotFound(result.Error);
            if (result.Error.Code == "Review.NotAuthorized") return StatusCode(StatusCodes.Status403Forbidden, result.Error);
            return BadRequest(result.Error);
        }
        return NoContent();
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);
        if (result.IsFailure) return result.Error.Code == "Review.NotFound" ? NotFound(result.Error) : BadRequest(result.Error);
        return Ok(result.Value);
    }

    [HttpGet("stadium/{stadiumId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStadiumReviews(int stadiumId)
    {
        var result = await _reviewService.GetStadiumReviewsAsync(stadiumId);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("my-reviews")]
    [Authorize(Policy = Permissions.Reviews_View)]
    public async Task<IActionResult> GetMyReviews()
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var result = await _reviewService.GetPlayerReviewsAsync(playerId);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpGet("can-review/{stadiumId}")]
    [Authorize(Policy = Permissions.Reviews_View)]
    public async Task<IActionResult> CanReview(int stadiumId)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId)) return Unauthorized();

        var result = await _reviewService.CanPlayerReviewStadiumAsync(stadiumId, playerId);
        return result.IsFailure ? BadRequest(result.Error) : Ok(new { CanReview = result.Value });
    }
}
