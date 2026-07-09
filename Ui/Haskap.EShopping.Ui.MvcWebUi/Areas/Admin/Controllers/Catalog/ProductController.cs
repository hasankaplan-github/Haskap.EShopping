using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Modules.Account.Domain.Shared.Consts;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.Product;

namespace Haskap.EShopping.Ui.MvcWebUi.Areas.Admin.Controllers.Catalog;

[Area("Admin")]
[Authorize(Permissions.Shop.Owner)]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly ISizeAttributeService _sizeAttributeService;
    private readonly IColorAttributeService _colorAttributeService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(
        IProductService catalogManagementService,
        ICategoryService categoryService,
        ISizeAttributeService sizeAttributeService,
        IColorAttributeService colorAttributeService,
        IWebHostEnvironment webHostEnvironment)
    {
        _productService = catalogManagementService;
        _categoryService = categoryService;
        _sizeAttributeService = sizeAttributeService;
        _colorAttributeService = colorAttributeService;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = await _categoryService.GetAllActiveCategoriesAsync(cancellationToken);

        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Search(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var result = await _productService.SearchAsync(input, jqueryDataTableParam, cancellationToken);
        return Json(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken cancellationToken = default)
    {
        var productDetails = await _productService.GetDetailsByIdAsync(id, cancellationToken);
        return View(productDetails);
    }

    public async Task<IActionResult> Update(Guid id, CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = await _categoryService.GetAllActiveCategoriesAsync(cancellationToken);

        var details = await _productService.GetDetailsByIdAsync(id, cancellationToken);

        return View(details);
    }

    [HttpPost]
    public async Task Update(UpdateInputDto input, CancellationToken cancellationToken = default)
    {
        await _productService.UpdateAsync(input, cancellationToken);
    }

    [HttpPost]
    public async Task Delete(Guid id, CancellationToken cancellationToken = default)
    {
        await _productService.DeleteAsync(id, cancellationToken);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        ViewBag.Categories = await _categoryService.GetAllActiveCategoriesAsync(cancellationToken);

        return View();
    }

    [HttpPost]
    public async Task<JsonResult> Create(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        var result = await _productService.CreateAsync(input, cancellationToken);
        return Json(result);
    }

    public async Task<IActionResult> CreateVariant(Guid productId, CancellationToken cancellationToken = default)
    {
        ViewBag.ProductId = productId;
        ViewBag.SizeAttributes = await _sizeAttributeService.GetAllAsync(cancellationToken);
        ViewBag.ColorAttributes = await _colorAttributeService.GetAllAsync(cancellationToken);

        return View();
    }

    [HttpPost]
    public async Task CreateVariant(CreateVariantInputDto input, List<IFormFile> photoFiles, CancellationToken cancellationToken = default)
    {
        if (photoFiles?.Any() == true)
        {
            var photoFilesTasks = photoFiles.AsParallel()
                .Select(async formFile =>
                {
                    var fileInputDto = new DddBase.Application.Dtos.Common.FileInputDto
                    {
                        ContentLength = formFile.Length,
                        OriginalName = formFile.FileName
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        await formFile.CopyToAsync(memoryStream);
                        fileInputDto.Content = memoryStream.ToArray();
                    }

                    return fileInputDto;
                });

            input.PhotoFiles = (await Task.WhenAll(photoFilesTasks)).ToList();
        }

        await _productService.CreateVariantAsync(input, _webHostEnvironment.ContentRootPath, cancellationToken);
    }

    public async Task<IActionResult> UpdateVariant(Guid productId, Guid variantId, CancellationToken cancellationToken = default)
    {
        ViewBag.SizeAttributes = await _sizeAttributeService.GetAllAsync(cancellationToken);
        ViewBag.ColorAttributes = await _colorAttributeService.GetAllAsync(cancellationToken);

        var variant = await _productService.GetVariantByIdAsync(productId, variantId, cancellationToken);

        return View(variant);
    }

    [HttpPost]
    public async Task UpdateVariant(UpdateVariantInputDto input, CancellationToken cancellationToken = default)
    {
        await _productService.UpdateVariantAsync(input, cancellationToken);
    }

    [HttpPost]
    public async Task DeleteVariant(DeleteVariantInputDto input, CancellationToken cancellationToken = default)
    {
        await _productService.DeleteVariantAsync(input, cancellationToken);
    }
}
