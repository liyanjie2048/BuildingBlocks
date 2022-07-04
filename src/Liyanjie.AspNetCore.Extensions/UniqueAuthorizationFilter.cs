using System;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Liyanjie.AspNetCore.Extensions;

public class UniqueAuthorizationFilter : IAuthorizationFilter
{
    public const string ClaimType_UniqueId = "UId";

    readonly Func<AuthorizationFilterContext, string> _getUserUniqueId;
    public UniqueAuthorizationFilter(Func<AuthorizationFilterContext, string> getUserUniqueIdFunc)
    {
        _getUserUniqueId = getUserUniqueIdFunc ?? throw new ArgumentNullException(nameof(getUserUniqueIdFunc));
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (true
            && context.ActionDescriptor.FilterDescriptors.Any(_ => _ is IAuthorizeData)
            && !context.ActionDescriptor.FilterDescriptors.Any(_ => _ is IAllowAnonymous)
            && context.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            var tokenUniqueId = context.HttpContext.User.Claims
                .SingleOrDefault(_ => _.Type == ClaimType_UniqueId)?.Value ?? Guid.NewGuid().ToString("N");
            if (tokenUniqueId is null)
                goto Unauthorized;

            var userUniqueId = _getUserUniqueId.Invoke(context);
            if (tokenUniqueId != userUniqueId)
                goto Unauthorized;

            Unauthorized:
            context.Result = new StatusCodeResult(401);
        }
    }
}
