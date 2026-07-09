using Haskap.DddBase.Utilities.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using Modules.Basket.Application.Contracts;
using Modules.Catalog.Application.Contracts;
using Modules.Catalog.Application.Dtos;
using Modules.Catalog.Domain.Shared.Consts;

namespace Haskap.EShopping.Ui.MvcWebUi.Controllers.Catalog;

public class CatalogController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ICatalogService _catalogService;
    private readonly VariantPhotoSettings _variantPhotoSettings;
    private readonly IBasketService _basketService;

    public CatalogController(
        ICatalogService catalogService,
        IWebHostEnvironment webHostEnvironment,
        IOptions<VariantPhotoSettings> variantPhotoSettingsOptions,
        IBasketService basketService)
    {
        _catalogService = catalogService;
        _webHostEnvironment = webHostEnvironment;
        _variantPhotoSettings = variantPhotoSettingsOptions.Value;
        _basketService = basketService;
    }

    public async Task<IActionResult> Index(SearchInputDto searchInput, CancellationToken cancellationToken = default)
    {
        ViewBag.SearchTerm = searchInput.SearchTerm;
        ViewBag.CurrentPageIndex = searchInput.CurrentPageIndex;
        ViewBag.PageSize = searchInput.PageSize;
        ViewBag.OrderBy = searchInput.OrderBy;
        ViewBag.FilterAttributes = searchInput.FilterAttributes;
        ViewBag.MinPriceValue = searchInput.MinPriceValue;
        ViewBag.MaxPriceValue = searchInput.MaxPriceValue;
        ViewBag.CategorySlug = searchInput.CategorySlug;

        var searchOutput = await _catalogService.SearchAsync(searchInput, cancellationToken);

        var pagination = new Pagination<SearchProductOutputDto>(searchOutput.Products.ToList(), searchInput.CurrentPageIndex, searchOutput.FilteredCount);

        ViewBag.Filters = searchOutput.Filters;

        return View(pagination);
    }

    public PhysicalFileResult GetProductPhotoPhysicalFile(Guid variantId, string fileName)
    {
        var fullFileName = Path.Combine([_webHostEnvironment.ContentRootPath, .. _variantPhotoSettings.FolderName.Split('\\'), variantId.ToString(), fileName]);
        new FileExtensionContentTypeProvider().TryGetContentType(fullFileName, out var contentType);
        return PhysicalFile(fullFileName, contentType ?? "image/jpg");
    }

    [HttpPost]
    public async Task AddToBasket(AddToBasketInputDto input, CancellationToken cancellationToken = default)
    {
        await _basketService.AddJustOneItemAsync(new() { ProductId = input.ProductId, ProductVariantId = input.ProductVariantId }, cancellationToken);
    }

    [Route("[controller]/[action]/{slugValue}")]
    public async Task<IActionResult> ProductDetails(string slugValue, CancellationToken cancellationToken = default)
    {
        var productDetails = await _catalogService.GetProductDetailsBySlugAsync(slugValue, cancellationToken);

        return View(productDetails);
    }
}
