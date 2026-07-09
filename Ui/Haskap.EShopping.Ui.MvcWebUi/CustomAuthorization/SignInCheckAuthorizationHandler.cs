using Haskap.DddBase.Presentation;
using Microsoft.AspNetCore.Authorization;
using Modules.Account.Application.Contracts.Account;

namespace Haskap.EShopping.Ui.MvcWebUi.CustomAuthorization;

public class SignInCheckAuthorizationHandler : IAuthorizationHandler
{
    private readonly IAccountService _accountService;

    public SignInCheckAuthorizationHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public async Task HandleAsync(AuthorizationHandlerContext context)
    {
        if (!context.User.TryFindUserId(out Guid userId))
        {
            context.Fail(new AuthorizationFailureReason(this, "Oturumunuz sonlanmış durumda. Yeniden giriş yapınız."));
            return;
        }

        if (!context.User.TryFindLoginId(out Guid loginId))
        {
            context.Fail(new AuthorizationFailureReason(this, "Oturumunuz sonlanmış durumda. Yeniden giriş yapınız."));
            return;
        }

        var currentOpenLogin = await _accountService.GetOpenLoginAsync(userId, loginId, default);

        if (currentOpenLogin is null)
        {
            context.Fail(new AuthorizationFailureReason(this, "Oturumunuz sonlanmış durumda. Yeniden giriş yapınız."));
            return;
        }

        await _accountService.SetLoginLastSeenDateTimeAsync(userId, loginId, currentOpenLogin.UtcLastSeenDateTime, default);

        context.Succeed(context.Requirements.First());
    }
}
