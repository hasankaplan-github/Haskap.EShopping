using Microsoft.AspNetCore.Mvc;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.ViewComponents.Shared;

public class AdminHeader : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(CancellationToken cancellationToken = default)
    {
        //return View($"~/Areas/Admin/Views/Shared/Components/Header/Default.cshtml");
        return View();
    }
}