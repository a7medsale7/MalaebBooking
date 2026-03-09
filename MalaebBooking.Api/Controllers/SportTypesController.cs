using MalaebBooking.Application.Contracts.SportTypes;
using MalaebBooking.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class SportTypesController : ControllerBase
{
    private readonly ISportTypeService _service;

    public SportTypesController(ISportTypeService service)
    {
        _service = service;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] SportTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value.Id },
            result.Value);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateSportTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> SoftDelete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);

        if (result.IsFailure)
            return NotFound(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }

    // ================== UPLOAD ICON ==================
    [HttpPost("{id:int}/icon")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadIcon(
        int id,
        [FromForm] UploadSportTypeIconRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.UploadIconAsync(id, request, cancellationToken); // ✅ مصحح

        if (result.IsFailure)
            return BadRequest(new { result.Error.Code, result.Error.Description });

        return Ok(result.Value);
    }
}
