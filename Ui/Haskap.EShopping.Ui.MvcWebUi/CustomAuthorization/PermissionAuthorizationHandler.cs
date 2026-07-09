using Haskap.DddBase.Presentation;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Microsoft.AspNetCore.Authorization;
using Modules.Account.Application.Contracts.Account;

namespace Haskap.EShopping.Ui.MvcWebUi.CustomAuthorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IAccountService _accountService;

    public PermissionAuthorizationHandler(IAccountService accountService)
    {
        _accountService = accountService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (!context.User.TryFindUserId(out Guid userId))
        {
            context.Fail();
            return;
        }

        var permissions = await _accountService.GetAllPermissionsAsync(new() { AccountId = userId });

        if (!permissions.Contains(requirement.Name))
        {
            context.Fail(new AuthorizationFailureReason(this, requirement.DisplayText ?? requirement.Name));
            return;
        }

        context.Succeed(requirement);
    }
}