using MalaebBooking.Domain.Consts;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Authentication.Filters;
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirements>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirements requirement)
    {
        // تحقق إذا المستخدم مسجل دخول وعنده الـ claim المطلوب
        if (context.User?.Identity is not { IsAuthenticated: true } ||
            !context.User.HasClaim(c => c.Type == Permissions.Type && c.Value == requirement.Permission))
        {
            return Task.CompletedTask; // لو ما تحقق الشرط نرجع Task.CompletedTask
        }

        context.Succeed(requirement); // المستخدم عنده الصلاحية
        return Task.CompletedTask; // لازم ترجع Task هنا برضو
    }
}