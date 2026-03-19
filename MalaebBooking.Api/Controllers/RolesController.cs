using MalaebBooking.Application.Contracts.Roles;
using MalaebBooking.Application.Services;
using MalaebBooking.Domain.Consts;
using MalaebBooking.Infrastructure.Authentication.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MalaebBooking.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Admin)] // تقييد الوصول للـ Admin فقط لإدارة الأدوار
public class RolesController(IRoleService _roleService) : ControllerBase
{
    private readonly IRoleService roleService = _roleService;

    // جلب كل الأدوار
    [HttpGet]
    [HasPermission(Permissions.Roles_View)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled = false)
    {
        var result = await roleService.GetAllAsync(includeDisabled);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // جلب دور معين بالتفاصيل والصلاحيات المرتبطة به
    [HttpGet("{id}")]
    [HasPermission(Permissions.Roles_View)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var result = await roleService.GetByIdAsync(id);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // جلب قائمة بكل الصلاحيات المتاحة في النظام
    // مفيدة جداً للـ Frontend عند عرض الـ Checkboxes لإضافة أو تعديل دور
    [HttpGet("permissions")]
    [HasPermission(Permissions.Roles_View)]
    public IActionResult GetPermissions()
    {
        var permissions = Permissions.GetAllPermissions();
        return Ok(permissions);
    }

    // إضافة دور جديد مع تحديد قائمة الصلاحيات الخاصة به
    [HttpPost]
    [HasPermission(Permissions.Roles_Create)]
    public async Task<IActionResult> Add([FromBody] RoleReqest request)
    {
        var result = await roleService.AddAsync(request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // تحديث اسم الدور أو تعديل قائمة الصلاحيات (Sync Permissions)
    [HttpPut("{id}")]
    [HasPermission(Permissions.Roles_Update)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] RoleReqest request)
    {
        var result = await roleService.UpdateAsync(id, request);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }

    // تفعيل أو تعطيل (Soft Delete) للدور
    [HttpPatch("{id}/toggle")]
    [HasPermission(Permissions.Roles_ToggleActive)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id)
    {
        var result = await roleService.ToggleStatusAsync(id);

        if (!result.IsSuccess)
            return BadRequest(result);

        return Ok(result);
    }
}
