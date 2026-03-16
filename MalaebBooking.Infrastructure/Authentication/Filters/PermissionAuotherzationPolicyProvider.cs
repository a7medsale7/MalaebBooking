using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MalaebBooking.Infrastructure.Authentication.Filters;
public class PermissionAuotherzationPolicyProvider(IOptions<AuthorizationOptions> options ): DefaultAuthorizationPolicyProvider(options)
{
    private readonly AuthorizationOptions _authorizationoptions = options.Value;
    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
       
        // لو policy موجودة في الخيارات العادية نرجعها
        var policy = await base.GetPolicyAsync(policyName);
        if (policy != null)
        {
            return policy;
        }
        // لو policy مش موجودة، نفترض إنها صلاحية وننشئ بوليصة جديدة
        var newPolicy = new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirements(policyName)) // نضيف الـ requirement اللي هو صلاحية
            .Build();
        _authorizationoptions.AddPolicy(policyName, newPolicy); // نضيف البوليصة الجديدة للخيارات
        return newPolicy; // نرجع البوليصة الجديدة

    }
}
