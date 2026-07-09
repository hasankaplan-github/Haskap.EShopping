using Microsoft.AspNetCore.Mvc;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.Shared;

public class Footer : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        return View();
    }
}