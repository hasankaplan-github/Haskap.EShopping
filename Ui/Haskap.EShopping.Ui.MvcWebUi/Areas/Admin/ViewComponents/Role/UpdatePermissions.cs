using Haskap.DddBase.Presentation.CustomAuthorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Application.Contracts.Role;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.ViewComponents.Role;

public class UpdatePermissions : ViewComponent
{
    private readonly IPermissionProvider _permissionProvider;
    private readonly IRoleService _roleService;

    public UpdatePermissions(
        IPermissionProvider permissionProvider, IRoleService roleService)
    {
        _permissionProvider = permissionProvider;
        _roleService = roleService;
    }

    public async Task<IViewComponentResult> InvokeAsync(Guid roleId, CancellationToken cancellationToken)
    {
        ViewBag.RoleId = roleId;

        var permissions = await _roleService.GetPermissionsAsync(roleId, cancellationToken);

        ViewBag.SelectedPermissions = permissions;

        var allPermissions = _permissionProvider.GetAllPermissions();

        return View(allPermissions);
    }
}
