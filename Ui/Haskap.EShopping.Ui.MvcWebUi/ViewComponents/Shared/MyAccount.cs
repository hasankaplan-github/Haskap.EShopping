using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.Shared.Consts;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.Shared;

public class MyAccount : ViewComponent
{
    private readonly IAccountService _accountService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public MyAccount(
        ICurrentUserIdProvider currentUserIdProvider,
        IAccountService accountService)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _accountService = accountService;
    }

    public async Task<IViewComponentResult> InvokeAsync(bool isMobile, CancellationToken cancellationToken = default)
    {
        if(User.Identity?.IsAuthenticated == true)
        {
            var getAllPermissionsInput = new GetAllPermissionsInputDto { AccountId = _currentUserIdProvider.CurrentUserId!.Value };
            var allPermissions = await _accountService.GetAllPermissionsAsync(getAllPermissionsInput, cancellationToken);

            if (allPermissions.Contains(Permissions.Shop.AdminPanelAccess))
            {
                ViewBag.HasAdminPanelAccess = true;
            }
        }

        if (isMobile)
        {
            return View("Mobile");
        }

        return View();
    }
}