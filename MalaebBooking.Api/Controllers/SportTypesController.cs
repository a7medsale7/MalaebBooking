// 4. SportTypesController.cs
using MalaebBooking.Application.Contracts.SportTypes;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SportTypesController(ISportTypeService service) : ControllerBase
{
    private readonly ISportTypeService _service = service;

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _service.GetAllAsync(cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var result = await _service.GetByIdAsync(id, cancellationToken);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.SportTypes_Create)]
    public async Task<IActionResult> Add([FromBody] SportTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.AddAsync(request, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.SportTypes_Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSportTypeRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UpdateAsync(id, request, cancellationToken);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.SportTypes_Delete)]
    public async Task<IActionResult> SoftDelete(int id, CancellationToken cancellationToken)
    {
        var result = await _service.SoftDeleteAsync(id, cancellationToken);
        return result.IsFailure ? NotFound(result.Error) : Ok(result.Value);
    }

    [HttpPost("{id:int}/icon")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = DefaultRoles.Admin, Policy = Permissions.SportTypes_UploadIcon)]
    public async Task<IActionResult> UploadIcon(int id, [FromForm] UploadSportTypeIconRequest request, CancellationToken cancellationToken)
    {
        var result = await _service.UploadIconAsync(id, request, cancellationToken);
        return result.IsFailure ? BadRequest(result.Error) : Ok(result.Value);
    }
}
