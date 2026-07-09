namespace Haskap.EShopping.Ui.MvcWebUi.Middlewares;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseAnonymousAccountProvider(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AnonymousAccountProviderMiddleware>();
    }
}
