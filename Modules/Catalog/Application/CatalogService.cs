using Haskap.DddBase.Application;
using Haskap.EShopping.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Contracts;
using Modules.Catalog.Application.Dtos;
using Modules.Catalog.Application.Mappings;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.ProductAggregate.Specifications;
using Modules.Catalog.Domain.Shared.Enums;
using System.Xml;

namespace Modules.Catalog.Application;

public class CatalogService : UseCaseService, ICatalogService
{
    private readonly ICatalogDbContext _catalogDbContext;

    public CatalogService(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task<ProductDto> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Include(p => p.Variants)
            .Where(p => p.Id == id)
            .FirstAsync(cancellationToken);

        return product.ToProductDto();
    }

    public async Task<ProductDto> GetProductBySkuAsync(string skuValue, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Include(p => p.Variants)
            .Where(p => p.Sku.Value == skuValue)
            .FirstAsync(cancellationToken);

        return product.ToProductDto();
    }

    public async Task<SearchOutputDto> SearchAsync(SearchInputDto searchInput, CancellationToken cancellationToken = default)
    {
        //var query = (from product in _catalogDbContext.Product

        //             join variant in _catalogDbContext.ProductVariant
        //                on product.Id equals variant.ProductId

        //             select new
        //             {
        //                 Product = product,
        //                 Variant = variant
        //             })
        //             .AsNoTracking();

        var query = _catalogDbContext.Product
            .AsNoTracking()
            .Include(p => p.Variants)
            .Include(x => x.Categories)
            .Where(x => x.Variants.Any());

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (!string.IsNullOrWhiteSpace(searchInput.SearchTerm))
        {
            filtered = true;

            query = query.Where(x =>
                x.SearchVectorTurkish.Matches(EF.Functions.WebSearchToTsQuery("turkish", searchInput.SearchTerm)) ||
                x.Variants.Any(v => 
                    v.SearchVectorTurkish.Matches(EF.Functions.WebSearchToTsQuery("turkish", searchInput.SearchTerm)) ||
                    v.Attributes.Any(a => a.SearchVectorTurkish.Matches(EF.Functions.WebSearchToTsQuery("turkish", searchInput.SearchTerm)))));
        }

        if (!string.IsNullOrWhiteSpace(searchInput.CategorySlug))
        {
            filtered = true;

            var category = await _catalogDbContext.Category
                .Where(c => c.Slug.Value == searchInput.CategorySlug)
                .FirstOrDefaultAsync(cancellationToken);

            if (category is not null)
            {
                query = query.Where(x => x.Categories.Any(c => c.CategoryId == category.Id));
            }
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        // No products found, return empty filters
        var filters = new FiltersOutputDto
        {
            AvailableAttributes = [],
            MinPriceValue = 0,
            MaxPriceValue = 0
        };

        if (filteredCount > 0)
        {
            // Get available filters before applying filters
            var filtersTemp = query
                .SelectMany(x => x.Variants)
                .GroupBy(v => 1)
                .Select(g => new
                {
                    AvailableAttributes = g.SelectMany(v => v.Attributes).ToList(),
                    MinPriceValue = g.Min(v => v.Price.Value),
                    MaxPriceValue = g.Max(v => v.Price.Value)
                }).First();

            filters = new FiltersOutputDto
            {
                AvailableAttributes = filtersTemp.AvailableAttributes.DistinctBy(x => new { x.AttributeType, x.DisplayName, x.Value }).Select(a => a.ToProductAttributeOutputDto()).Distinct().ToList(),
                MinPriceValue = filtersTemp.MinPriceValue,
                MaxPriceValue = filtersTemp.MaxPriceValue
            };
        }


        // Apply filters
        if (searchInput.FilterAttributes?.Any() == true)
        {
            var filterAttributes = searchInput.FilterAttributes.ToProductAttributeOutputDtos();

            if (filterAttributes.Any() == true)
            {
                filtered = true;

                query = query.Where(new AttributeFilterSpecification(filterAttributes));

                //query = query.Where(x =>
                //    x.Variants.Any(y =>
                //        y.Attributes.Any(z =>
                //            filterAttributes.Any(a =>
                //                a.AttributeType == z.AttributeType &&
                //                a.DisplayName == z.DisplayName &&
                //                $"#{a.Value}" == z.Value))));
            }
        }

        if(searchInput.MinPriceValue.HasValue)
        {
            filtered = true;
            query = query.Where(x => x.Variants.Any(v => v.Price.Value >= searchInput.MinPriceValue));
        }

        if(searchInput.MaxPriceValue.HasValue)
        {
            filtered = true;
            query = query.Where(x => x.Variants.Any(v => v.Price.Value <= searchInput.MaxPriceValue));
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        query = searchInput.OrderBy switch
        {
            OrderBy.Newest => query.OrderByDescending(x => x.ModifiedOnUtc ?? x.CreatedOnUtc),
            OrderBy.PriceAsc => query.OrderBy(x => x.Variants.Min(v => v.Price.Value)),
            OrderBy.PriceDesc => query.OrderByDescending(x => x.Variants.Max(v => v.Price.Value)),
            _ => query
        };

        var products = await query
            .Skip((searchInput.CurrentPageIndex - 1) * searchInput.PageSize)
            .Take(searchInput.PageSize)
            .Select(x => new SearchProductOutputDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                SkuValue = x.Sku.Value,
                IsActive = x.IsActive,
                PrimaryVariant = x.Variants.Where(v => v.IsPrimary).First().ToProductVariantDto()
            }).ToListAsync(cancellationToken);

        return new()
        {
            Products = products.AsReadOnly(),
            Filters = filters,
            TotalCount = totalCount,
            FilteredCount = filteredCount
        };
    }

    public async Task SellAsync(SellInputDto input, CancellationToken cancellationToken = default)
    {
        foreach (var itemGrouping in input.Items.GroupBy(x => x.ProductId))
        {
            var variantIds = itemGrouping.Select(x => x.VariantId).ToList();

            var product = await _catalogDbContext.Product
                .Include(p => p.Variants.Where(v => variantIds.Contains(v.Id)))
                .Where(p => p.Id == itemGrouping.Key)
                .FirstAsync(cancellationToken);

            foreach (var item in itemGrouping)
            {
                var variant = product.Variants.Where(x => x.Id == item.VariantId).First();
                variant.Sell(item.Quantity);
            }
        }

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ItemForBasketOutputDto>> GetItemsForBasketAsync(Guid productId, IList<Guid> variantIds, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .AsNoTracking()
            .Include(p => p.Categories)
            .Include(p => p.Variants.Where(v => variantIds.Contains(v.Id)))
            .Where(p => p.Id == productId)
            .FirstOrDefaultAsync(cancellationToken);

        return product?.Variants.Select(v => new ItemForBasketOutputDto
        {
            VariantId = v.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            VariantSkuValue = v.Sku.Value,
            SlugValue = v.Slug.Value,
            PrimaryPicture = v.PrimaryPicture.ToProductPictureDto(),
            Price = v.Price.ToMoneyOutputDto(),
            CategoryIds = product.Categories.Select(c => c.CategoryId).ToList()
        }).ToList() ?? [];
    }

    public async Task<ProductDetailsOutputDto> GetProductDetailsBySlugAsync(string slugValue, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(slugValue))
        {
            throw new ArgumentException("Ürün bulunamadı!", nameof(slugValue));
        }

        var productDetails = await _catalogDbContext.Product
            .Include(x => x.Variants)
            .Where(x => x.Variants.Any(y => y.Slug.Value == slugValue))
            .Select(x => new ProductDetailsOutputDto
            {
                Product = x.ToProductDto(),
                SelectedVariant = x.Variants.Where(y => y.Slug.Value == slugValue).FirstOrDefault()!.ToProductVariantDto()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (productDetails is null)
        {
            throw new ArgumentException("Ürün bulunamadı!", nameof(slugValue));
        }

        return productDetails;
    }
}
