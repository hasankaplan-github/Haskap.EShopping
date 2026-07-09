using Haskap.DddBase.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace Haskap.EShopping.Ui.MvcWebUi.CustomAuthorization;

public class PermissionAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Forbidden)
        {
            var failedPermissions = authorizeResult.AuthorizationFailure?.FailureReasons
                .Where(x => x.Handler is PermissionAuthorizationHandler)
                .Select(x => x.Message);

            if (failedPermissions?.Any(x => !string.IsNullOrWhiteSpace(x)) == true)
            {
                throw new ForbiddenOperationException(string.Join(',', failedPermissions));
            }

            if (authorizeResult.AuthorizationFailure?.FailureReasons.Any(x => x.Handler is SignInCheckAuthorizationHandler) == true)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                context.Response.Redirect("/Account/Login");
                return;
            }
        }

        await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}
