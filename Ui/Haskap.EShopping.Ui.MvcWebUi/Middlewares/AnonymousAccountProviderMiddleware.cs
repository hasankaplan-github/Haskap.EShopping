using Haskap.EShopping.Domain.Providers;
using Haskap.EShopping.Domain.Shared.Consts;

namespace Haskap.EShopping.Ui.MvcWebUi.Middlewares;

public class AnonymousAccountProviderMiddleware
{
    private readonly RequestDelegate _next;

    public AnonymousAccountProviderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IAnonymousAccountProvider anonymousAccountProvider)
    {
        anonymousAccountProvider.AccountId = Guid.TryParse(httpContext.Request.Cookies[AnonymousAccountConsts.AccountIdCookieName], out var anonymousAccountId)
                ? anonymousAccountId
                : Guid.NewGuid();

        httpContext.Response.Cookies.Append(
            AnonymousAccountConsts.AccountIdCookieName,
            anonymousAccountProvider.AccountId.ToString(),
            new CookieOptions
            {
                Expires = DateTimeOffset.MaxValue,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Secure = true
            });

        await _next(httpContext);
    }
}
