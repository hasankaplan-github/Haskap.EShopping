using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.Shared.Consts;

namespace Haskap.EShopping.Ui.MvcWebUi.Controllers;

public class HomeController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public HomeController(
        IAccountService accountService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _accountService = accountService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    [DisableRateLimiting]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error(CancellationToken cancellationToken = default)
    {
        var errorEnvelope = HttpContext.Items["Envelope"] as Envelope;
        //if (User.Identity?.IsAuthenticated == true)
        //{
        //    var getAllPermissionsInput = new GetAllPermissionsInputDto { AccountId = _currentUserIdProvider.CurrentUserId!.Value };
        //    var allPermissions = await _accountService.GetAllPermissionsAsync(getAllPermissionsInput, cancellationToken);

        //    if (allPermissions.Contains(Permissions.Shop.AdminPanelAccess))
        //    {
        //        return View("~/Areas/Admin/Views/Shared/Error.cshtml", errorEnvelope);
        //    }
        //}

        return View(errorEnvelope);
    }
}
