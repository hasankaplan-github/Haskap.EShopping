using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Shared.Consts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Application.Contracts.Role;
using Modules.Account.Application.Dtos.Role;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.Controllers.Role;

[Area("Admin")]
[Authorize]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [Authorize(AdminPermissions.Role.Read)]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View();
    }

    [Authorize(AdminPermissions.Role.Read)]
    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default) 
    {
        var result = await _roleService.SearchAsync(inputDto, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    [Authorize(AdminPermissions.Role.Create)]
    [HttpPost]
    public async Task SaveNew(SaveNewInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _roleService.SaveNewAsync(inputDto, cancellationToken);
    }

    [Authorize(AdminPermissions.Role.Delete)]
    [HttpDelete]
    public async Task Delete(DeleteInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _roleService.DeleteAsync(inputDto, cancellationToken);
    }

    [Authorize(AdminPermissions.Role.Read)]
    [HttpGet]
    public async Task<JsonResult> GetById(Guid roleId, CancellationToken cancellationToken = default)
    {
        var output = await _roleService.GetByIdAsync(roleId, cancellationToken);

        return Json(output);
    }

    [Authorize(AdminPermissions.Role.Update)]
    [HttpPut]
    public async Task Update(UpdateInputDto inputDto, CancellationToken cancellationToken = default)
    {
        await _roleService.UpdateAsync(inputDto, cancellationToken);
    }

    [Authorize(AdminPermissions.Role.UpdatePermissions)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdatePermissionsViewComponent(Guid roleId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Role.UpdatePermissions), new { roleId });
    }

    [Authorize(AdminPermissions.Role.UpdatePermissions)]
    [HttpPost]
    public async Task UpdatePermissions(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        await _roleService.UpdatePermissionsAsync(inputDto, cancellationToken);
    }

}

