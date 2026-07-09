using Microsoft.AspNetCore.Mvc;

namespace Haskap.EShopping.Ui.MvcWebUi.ViewComponents.Shared;

public class HeroSection : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string? title, CancellationToken cancellationToken = default)
    {
        ViewBag.Title = title;

        return View();
    }
}