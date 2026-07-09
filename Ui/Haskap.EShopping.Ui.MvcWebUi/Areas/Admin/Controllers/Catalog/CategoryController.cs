using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Domain.Shared.Consts;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.Category;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.Controllers.Catalog;

[Area("Admin")]
[Authorize(Permissions.Shop.Owner)]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var result = await _categoryService.SearchAsync(input, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task Create(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        await _categoryService.CreateAsync(input, cancellationToken);
    }

    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken = default)
    {
        var category = await _categoryService.GetByIdAsync(id, cancellationToken);
        return View(category);
    }

    [HttpPost]
    public async Task Update(UpdateInputDto input, CancellationToken cancellationToken = default)
    {
        await _categoryService.UpdateAsync(input, cancellationToken);
    }

    [HttpPost]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _categoryService.DeleteAsync(id, cancellationToken);
    }
}
