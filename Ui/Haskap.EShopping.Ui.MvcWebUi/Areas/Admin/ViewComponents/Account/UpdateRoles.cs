using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Contracts.Role;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.Shared.Enums;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.ViewComponents.Account;

public class UpdateRoles : ViewComponent
{
    private readonly IRoleService _roleService;
    private readonly IAccountService _accountService;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;

    public UpdateRoles(
        IRoleService roleService,
        IAccountService accountService,
        ICurrentUserIdProvider currentUserIdProvider)
    {
        _roleService = roleService;
        _accountService = accountService;
        _currentUserIdProvider = currentUserIdProvider;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid? userId, UserOptionsEditType userOptionsEditType, CancellationToken cancellationToken)
    {
        var currentUserId = userOptionsEditType == UserOptionsEditType.UserSelfEdit ? _currentUserIdProvider.CurrentUserId!.Value : userId.Value;
        ViewBag.UserId = currentUserId;
        ViewBag.UserOptionsEditType = userOptionsEditType;


        var roles = await _accountService.GetRolesAsync(new GetRolesInputDto{ UserId = currentUserId }, cancellationToken);

        ViewBag.SelectedRoles = roles;

        return View(await _roleService.GetAllAsync(cancellationToken));
    }
}
