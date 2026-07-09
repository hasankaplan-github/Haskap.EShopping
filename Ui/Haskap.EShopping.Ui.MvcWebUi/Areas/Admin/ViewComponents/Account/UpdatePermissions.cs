using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Presentation.CustomAuthorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.Shared.Enums;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.ViewComponents.Account;

public class UpdatePermissions : ViewComponent
{
    private readonly IPermissionProvider _permissionProvider;
    private readonly IAccountService _accountService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public UpdatePermissions(
        IPermissionProvider permissionProvider,
        IAccountService accountService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _permissionProvider = permissionProvider;
        _accountService = accountService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid? userId, UserOptionsEditType userOptionsEditType, CancellationToken cancellationToken)
    {
        var currentUserId = userOptionsEditType == UserOptionsEditType.UserSelfEdit ? _currentUserIdProvider.CurrentUserId!.Value : userId.Value;
        ViewBag.UserId = currentUserId;
        ViewBag.UserOptionsEditType = userOptionsEditType;


        var permissions = await _accountService.GetUserPermissionsAsync(new GetUserPermissionsInputDto { UserId = currentUserId }, cancellationToken);

        ViewBag.SelectedPermissions = permissions;

        var allPermissions = _permissionProvider.GetAllPermissions();

        return View(allPermissions);
    }
}
