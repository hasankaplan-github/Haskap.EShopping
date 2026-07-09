using Haskap.DddBase.Domain.Providers;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Application.Contracts.Account;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.PersonalInfoSummary;

public class PersonalInfoSummary : ViewComponent
{
    private readonly ICurrentUserIdProvider _currentUserIdProvider;
    private readonly IAccountService _accountService;

    public PersonalInfoSummary(
        ICurrentUserIdProvider currentUserIdProvider,
        IAccountService accountService)
    {
        _currentUserIdProvider = currentUserIdProvider;
        _accountService = accountService;
    }

    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserIdProvider.CurrentUserId is not null)
        {
            var account = await _accountService.GetByIdAsync(_currentUserIdProvider.CurrentUserId.Value, cancellationToken);
            return View("AccountPersonalInfo", account);
        }

        return View();
    }
}