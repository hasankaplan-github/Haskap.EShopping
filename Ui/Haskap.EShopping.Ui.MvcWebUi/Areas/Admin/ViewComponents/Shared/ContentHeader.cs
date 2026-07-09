using Microsoft.AspNetCore.Mvc;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.ViewComponents.Shared;

public class ContentHeader : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string? title, CancellationToken cancellationToken = default)
    {
        ViewBag.Title = title;
        return View();
    }
}