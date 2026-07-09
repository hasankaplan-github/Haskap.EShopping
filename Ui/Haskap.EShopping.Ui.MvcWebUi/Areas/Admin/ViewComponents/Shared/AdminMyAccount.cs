using Microsoft.AspNetCore.Mvc;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.ViewComponents.Shared;

public class AdminMyAccount : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        return View();
    }
}