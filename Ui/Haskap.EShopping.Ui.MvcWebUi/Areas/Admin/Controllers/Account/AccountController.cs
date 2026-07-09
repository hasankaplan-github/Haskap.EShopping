using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Common.Exceptions;
using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.Shared.Enums;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.Controllers.Account;

[Area("Admin")]
[Authorize]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ICurrentTenantProvider _currentTenantProvider;
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IAuthorizationService _authorizationService;
    private readonly IIsActiveGlobalQueryFilterProvider _isActive;
    private readonly IMemoryCache _memoryCache;

    public AccountController(
        IAccountService accountService,
        ICurrentTenantProvider currentTenantProvider,
        ICurrentUserIdProvider currentUserIdProvider,
        IAuthorizationService authorizationService,
        IIsActiveGlobalQueryFilterProvider isActive,
        IMemoryCache memoryCache)
    {
        _accountService = accountService;
        _currentTenantProvider = currentTenantProvider;
        _currentUserIdProvider = currentUserIdProvider;
        _authorizationService = authorizationService;
        _isActive = isActive;
        _memoryCache = memoryCache;
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.ReadWithinSameTenant)]
    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var result = await _accountService.SearchAsync(inputDto, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    [Authorize(Modules.Account.Domain.Shared.Consts.AdminPermissions.Account.Create)]
    [HttpGet]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        return View();
    }

    [Authorize(Modules.Account.Domain.Shared.Consts.AdminPermissions.Account.Create)]
    [HttpPost]
    public async Task<JsonResult> Create(CreateInputDto inputDto, CancellationToken cancellationToken = default)
    {
        var result = await _accountService.CreateAsync(inputDto, cancellationToken);
        return Json(result);
    }

    [HttpPost]
    public async Task SignOutOfOpenLoginOfCurrentUser(Guid openLoginId, CancellationToken cancellationToken = default)
    {
        await _accountService.SignOutOfOpenLoginAsync(_currentUserIdProvider.CurrentUserId!.Value, openLoginId, cancellationToken);
    }


    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Profile.ReadWithinSameTenant)]
    [HttpGet]
    public async Task<JsonResult> GetOpenLoginsOfUserWithinSameTenant(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await _accountService.GetOpenLoginsOfUserAsync(_currentTenantProvider.CurrentTenantId, userId, cancellationToken);
        return Json(result);
    }

    [HttpGet]
    public async Task<JsonResult> GetOpenLoginsOfCurrentUser(CancellationToken cancellationToken = default)
    {
        var result = await _accountService.GetOpenLoginsOfUserAsync(_currentTenantProvider.CurrentTenantId, _currentUserIdProvider.CurrentUserId!.Value, cancellationToken);
        return Json(result);
    }

    public async Task<IActionResult> CurrentUserProfile(CancellationToken cancellationToken)
    {
        using var _ = _isActive.Disable();

        var account = await _accountService.GetByIdAsync(_currentUserIdProvider.CurrentUserId.Value, cancellationToken);

        ViewBag.IsCurrentUserProfile = true;
        ViewBag.UserOptionsEditType = UserOptionsEditType.UserSelfEdit;

        return View("Profile", account);
    }


    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Profile.ReadWithinSameTenant)]
    public async Task<IActionResult> Profile(Guid userId, CancellationToken cancellationToken)
    {
        using var __ = _isActive.Disable();

        var account = await _accountService.GetByIdAsync(userId, cancellationToken);

        ViewBag.IsCurrentUserProfile = _currentUserIdProvider.CurrentUserId.Value == userId;
        ViewBag.UserOptionsEditType = UserOptionsEditType.EditWithinSameTenant;

        return View(account);
    }


    [HttpPost]
    public async Task ChangePassword(ChangePasswordInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.ChangePasswordAsync(inputDto, cancellationToken);
    }

    [HttpPut]
    public async Task Update(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.UpdateAsync(inputDto, cancellationToken);
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.ReadWithinSameTenant)]
    [HttpGet]
    public async Task<IActionResult> Accounts(CancellationToken cancellationToken)
    {
        return View();
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.Read)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdatePermissionsViewComponent(CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Account.UpdatePermissions), new { UserOptionsEditType = UserOptionsEditType.UserSelfEdit });
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.ReadWithinSameTenant)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdatePermissionsViewComponentWithSameTenant(Guid userId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Account.UpdatePermissions), new { UserId = userId, UserOptionsEditType = UserOptionsEditType.EditWithinSameTenant });
    }

    private void DetectInvalidPermissionNameAndThrowIfAny(List<string> updatedPermissionNames)
    {
        var allDefinedPermissionNames = (new CustomAuthorization.PermissionProvider()).GetAllPermissions()
            .Values
            .SelectMany(x => x)
            .Select(x => x.Name)
            .ToHashSet();

        foreach (var permissionName in updatedPermissionNames)
        {
            if (allDefinedPermissionNames.Contains(permissionName) == false)
            {
                throw new InvalidOperationException();
            }
        }
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.Update)]
    [HttpPost]
    public async Task UpdatePermissions(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        if ((await _authorizationService.AuthorizeAsync(User, Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.App.Admin)).Succeeded == false)
        {
            var hasInvalidPermissions = inputDto.UncheckedPermissions.Any(x => x.StartsWith($"{nameof(Modules.Account.Domain.Shared.Consts.AdminPermissions)}")) ||
                                        inputDto.CheckedPermissions.Any(x => x.StartsWith($"{nameof(Modules.Account.Domain.Shared.Consts.AdminPermissions)}"));

            if (hasInvalidPermissions)
            {
                throw new ForbiddenOperationException(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.App.Admin);
            }
        }

        DetectInvalidPermissionNameAndThrowIfAny([.. inputDto.UncheckedPermissions, .. inputDto.CheckedPermissions]);

        inputDto.UserId = _currentUserIdProvider.CurrentUserId;

        await _accountService.UpdatePermissionsAsync(inputDto, cancellationToken);
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Permission.UpdateWithinSameTenant)]
    [HttpPost]
    public async Task UpdatePermissionsWithinSameTenant(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        if ((await _authorizationService.AuthorizeAsync(User, Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.App.Admin)).Succeeded == false)
        {
            var hasInvalidPermissions = inputDto.UncheckedPermissions.Any(x => x.StartsWith($"{nameof(Modules.Account.Domain.Shared.Consts.AdminPermissions)}")) ||
                                        inputDto.CheckedPermissions.Any(x => x.StartsWith($"{nameof(Modules.Account.Domain.Shared.Consts.AdminPermissions)}"));

            if (hasInvalidPermissions)
            {
                throw new ForbiddenOperationException(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.App.Admin);
            }
        }

        DetectInvalidPermissionNameAndThrowIfAny([.. inputDto.UncheckedPermissions, .. inputDto.CheckedPermissions]);

        await _accountService.UpdatePermissionsAsync(inputDto, cancellationToken);
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Role.Read)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdateRolesViewComponent(Guid userId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Account.UpdateRoles), new { UserOptionsEditType = UserOptionsEditType.UserSelfEdit });
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Role.ReadWithinSameTenant)]
    [HttpGet]
    public async Task<IActionResult> LoadUpdateRolesViewComponentWithSameTenant(Guid userId, CancellationToken cancellationToken)
    {
        return ViewComponent(typeof(ViewComponents.Account.UpdateRoles), new { userId, UserOptionsEditType = UserOptionsEditType.EditWithinSameTenant });
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Role.Update)]
    [HttpPost]
    public async Task UpdateRoles(UpdateRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        inputDto.UserId = _currentUserIdProvider.CurrentUserId;

        await _accountService.UpdateRolesAsync(inputDto, cancellationToken);
    }

    [Authorize(Haskap.DddBase.Domain.Shared.Consts.AdminPermissions.Account.Role.UpdateWithinSameTenant)]
    [HttpPost]
    public async Task UpdateRolesWithinSameTenant(UpdateRolesInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.UpdateRolesAsync(inputDto, cancellationToken);
    }

    [Authorize(Modules.Account.Domain.Shared.Consts.AdminPermissions.Account.ResetFailedLoginAttemptsAndUnlockWithinSameTenant)]
    [HttpPut]
    public async Task ResetFailedLoginAttemptsAndUnlockWithinSameTenant(ResetFailedLoginAttemptsAndUnlockInputDto inputDto, CancellationToken cancellationToken)
    {
        await _accountService.ResetFailedLoginAttemptsAndUnlockAsync(inputDto, cancellationToken);
    }
}
