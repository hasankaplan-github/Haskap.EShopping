using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Modules.Account.Application.Contracts.Account;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Domain.Shared.Enums;
using System.Security.Claims;

namespace Haskap.EShopping.Ui.MvcWebUi.Controllers.Account;


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

    [HttpPost]
    public async Task SignOutOfOpenLoginOfCurrentUser(Guid openLoginId, CancellationToken cancellationToken = default)
    {
        await _accountService.SignOutOfOpenLoginAsync(_currentUserIdProvider.CurrentUserId!.Value, openLoginId, cancellationToken);
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

    [AllowAnonymous]
    public async Task<IActionResult> Login(string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        returnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/";

        if (User.Identity?.IsAuthenticated == true)
        {            
            return LocalRedirect(returnUrl);
        }

        ViewBag.ReturnUrl = returnUrl;

        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task SignIn(LoginInputDto inputDto, CancellationToken cancellationToken = default)
    {
        inputDto.UserAgentString = Request.Headers.UserAgent.ToString();
        inputDto.RemoteIpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        //inputDto.Platform = Request.Headers["sec-ch-ua-platform"].ToString();
        //inputDto.Browser = Request.Headers["sec-ch-ua"].ToString();
        //inputDto.IsMobile = Request.Headers["sec-ch-ua-mobile"].ToString() == "?1";

        var output = await _accountService.LoginAsync(inputDto, cancellationToken);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, inputDto.Username),
            new Claim(ClaimTypes.GivenName, output.UserFirstName),
            new Claim(ClaimTypes.Surname, output.UserLastName),
            new Claim(ClaimTypes.NameIdentifier, output.AccountId.ToString()),

            new Claim(Haskap.DddBase.Domain.Shared.Consts.AccountConsts.LoginIdClaimType, output.LoginId.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            //AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(5), //.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            IsPersistent = false,
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            IssuedUtc = DateTimeOffset.UtcNow
            // The time at which the authentication ticket was issued.

            //RedirectUri = <string>
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    public async Task<IActionResult> Logout(string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        returnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : "/";

        if (returnUrl.Contains("Home/Error", StringComparison.OrdinalIgnoreCase))
        {
            returnUrl = "/";
        }

        if (User.Identity!.IsAuthenticated == true)
        {
            // Clear the existing external cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            await _accountService.SignOutAsync(cancellationToken);
        }

        return LocalRedirect(returnUrl);
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
}
