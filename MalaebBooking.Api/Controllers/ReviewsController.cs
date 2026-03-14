using MalaebBooking.Application.Contracts.Reviews;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // POST: api/reviews
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewRequest request)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var result = await _reviewService.CreateReviewAsync(request, playerId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { Message = "تم إضافة التقييم بنجاح." });
    }

    // PUT: api/reviews/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewRequest request)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var result = await _reviewService.UpdateReviewAsync(id, request, playerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Review.NotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Review.NotAuthorized")
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);

            return BadRequest(result.Error);
        }

        return Ok(new { Message = "تم تعديل التقييم بنجاح." });
    }

    // DELETE: api/reviews/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var result = await _reviewService.DeleteReviewAsync(id, playerId);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Review.NotFound")
                return NotFound(result.Error);

            if (result.Error.Code == "Review.NotAuthorized")
                return StatusCode(StatusCodes.Status403Forbidden, result.Error);

            return BadRequest(result.Error);
        }

        return NoContent();
    }

    // GET: api/reviews/{id}
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _reviewService.GetReviewByIdAsync(id);

        if (result.IsFailure)
        {
            if (result.Error.Code == "Review.NotFound")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    // GET: api/reviews/stadium/{stadiumId}
    [HttpGet("stadium/{stadiumId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetStadiumReviews(int stadiumId)
    {
        var result = await _reviewService.GetStadiumReviewsAsync(stadiumId);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    // GET: api/reviews/my-reviews
    [HttpGet("my-reviews")]
    public async Task<IActionResult> GetMyReviews()
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var result = await _reviewService.GetPlayerReviewsAsync(playerId);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    // GET: api/reviews/can-review/{stadiumId}
    [HttpGet("can-review/{stadiumId}")]
    public async Task<IActionResult> CanReview(int stadiumId)
    {
        var playerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(playerId))
            return Unauthorized();

        var result = await _reviewService.CanPlayerReviewStadiumAsync(stadiumId, playerId);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return Ok(new { CanReview = result.Value });
    }
}
