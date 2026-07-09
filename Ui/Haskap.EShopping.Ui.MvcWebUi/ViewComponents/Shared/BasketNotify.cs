using Microsoft.AspNetCore.Mvc;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.Shared;

public class BasketNotify : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(bool isMobile, CancellationToken cancellationToken = default)
    {
        if (isMobile)
        {
            return View("Mobile");
        }

        return View();
    }
}