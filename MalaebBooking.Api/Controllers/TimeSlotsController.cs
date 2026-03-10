using MalaebBooking.Application.Contracts.TimeSlots;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize] // يحمي كل الـ Endpoints في الكنترولر ده مبدئياً
public class TimeSlotsController : ControllerBase
{
    private readonly ITimeSlotService _timeSlotService;

    public TimeSlotsController(ITimeSlotService timeSlotService)
    {
        _timeSlotService = timeSlotService;
    }

    [HttpGet("stadium/{stadiumId}")]
    public async Task<IActionResult> GetAllByStadiumId(int stadiumId)
    {
        var result = await _timeSlotService.GetAllByStadiumAsync(stadiumId);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
            
        return Ok(result.Value);
    }

    [HttpGet("stadium/{stadiumId}/available")]
    [AllowAnonymous] // يسمح لأي حد (حتى لو مش مسجل دخول) إنه يشوف المواعيد المتاحة للحجز
    public async Task<IActionResult> GetAvailableByStadiumAndDate(int stadiumId, [FromQuery] DateOnly date)
    {
        var result = await _timeSlotService.GetAvailableByStadiumAndDateAsync(stadiumId, date);
        
        if (result.IsFailure)
            return BadRequest(result.Error);
            
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    [AllowAnonymous] // يسمح لأي حد إنه يعرض تفاصيل ميعاد معين لو أحتاجنا ده في الـ UI
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _timeSlotService.GetByIdAsync(id);
        
        if (result.IsFailure)
        {
            if (result.Error.Code == "TimeSlot.NotFound")
                return NotFound(result.Error);
                
            return BadRequest(result.Error);
        }
        
        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateTimeSlotRequest request)
    {
        var result = await _timeSlotService.CreateTimeSlotAsync(request);
        
        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTimeSlotRequest request)
    {
        var result = await _timeSlotService.UpdateTimeSlotAsync(id, request);
        
        if (result.IsFailure)
        {
            if (result.Error.Code == "TimeSlot.NotFound")
                return NotFound(result.Error);
                
            return BadRequest(result.Error);
        }
            
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _timeSlotService.DeleteTimeSlotAsync(id);
        
        if (result.IsFailure)
        {
             if (result.Error.Code == "TimeSlot.NotFound")
                return NotFound(result.Error);
                
            return BadRequest(result.Error);
        }
            
        return NoContent();
    }
}
