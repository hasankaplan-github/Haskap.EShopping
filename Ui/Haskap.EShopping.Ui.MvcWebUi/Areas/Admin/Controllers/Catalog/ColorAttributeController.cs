using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Domain.Shared.Consts;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.ColorAttribute;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.Controllers.Catalog;

[Area("Admin")]
[Authorize(Permissions.Shop.Owner)]
public class ColorAttributeController : Controller
{
    private readonly IColorAttributeService _colorAttributeService;

    public ColorAttributeController(IColorAttributeService colorAttributeService)
    {
        _colorAttributeService = colorAttributeService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var result = await _colorAttributeService.SearchAsync(input, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task Create(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        await _colorAttributeService.CreateAsync(input, cancellationToken);
    }

    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken = default)
    {
        var sizeAttribute = await _colorAttributeService.GetByIdAsync(id, cancellationToken);
        return View(sizeAttribute);
    }

    [HttpPost]
    public async Task Update(UpdateInputDto input, CancellationToken cancellationToken = default)
    {
        await _colorAttributeService.UpdateAsync(input, cancellationToken);
    }

    [HttpPost]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _colorAttributeService.DeleteAsync(id, cancellationToken);
    }
}
