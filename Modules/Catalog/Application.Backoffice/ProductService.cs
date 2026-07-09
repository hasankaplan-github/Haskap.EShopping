using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.Product;
using Modules.Catalog.Application.Backoffice.Mappings;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.ProductAggregate;
using Modules.Catalog.Domain.ProductAggregate.Events;
using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Application.Backoffice;

public class ProductService : UseCaseService, IProductService
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly IIsActiveGlobalQueryFilterProvider _isActiveDataFilter;
    private readonly IEventPublisher _eventPublisher;

    public ProductService(
        ICatalogDbContext catalogDbContext,
        IIsActiveGlobalQueryFilterProvider isActiveDataFilter,
        IEventPublisher eventPublisher)
    {
        _catalogDbContext = catalogDbContext;
        _isActiveDataFilter = isActiveDataFilter;
        _eventPublisher = eventPublisher;
    }

    public async Task<SearchOutputDto> CreateAsync(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        var newProduct = new Product(GuidGenerator.CreateSimpleGuid(), input.Name, input.Description, input.IsActive);
        newProduct.AddCategories(input.CategoryIds);

        _catalogDbContext.Product.Add(newProduct);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        return newProduct.ToSearchOutputDto();
    }

    public async Task AddStockAsync(AddStockInputDto input, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Include(p => p.Variants.Where(v => v.Id == input.VariantId))
            .Where(p => p.Id == input.ProductId)
            .FirstAsync(cancellationToken);

        product.Variants[0].AddStock(input.Quantity);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> CopyAsync(CopyInputDto input, CancellationToken cancellationToken = default)
    {
        using var _ = _isActiveDataFilter.Disable();

        var product = await _catalogDbContext.Product
            .Include(p => p.Variants)
            .Include(p => p.Categories)
            .Where(p => p.Id == input.ProductIdToBeCopied)
            .FirstAsync(cancellationToken);

        var newProductId = product.Copy(GuidGenerator.CreateSimpleGuid(), input.Name, input.Description, input.IsActive);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        return newProductId;
    }

    public async Task EmptyStockAsync(EmptyStockInputDto input, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Include(p => p.Variants.Where(v => v.Id == input.VariantId))
            .Where(p => p.Id == input.ProductId)
            .FirstAsync(cancellationToken);

        product.Variants[0].EmptyStock();

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        using var _ = _isActiveDataFilter.Disable();

        var query = _catalogDbContext.Product
            .AsNoTracking()
            .Include(p => p.Variants)
            .Include(x => x.Categories)
            .AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;

        if (!string.IsNullOrWhiteSpace(input.SearchTerm))
        {
            filtered = true;

            query = query.Where(x =>
                x.SearchVectorTurkish.Matches(EF.Functions.WebSearchToTsQuery("turkish", input.SearchTerm)) ||
                x.Variants.Any(v =>
                    v.SearchVectorTurkish.Matches(EF.Functions.WebSearchToTsQuery("turkish", input.SearchTerm)) ||
                    v.Attributes.Any(a => a.SearchVectorTurkish.Matches(EF.Functions.WebSearchToTsQuery("turkish", input.SearchTerm)))) ||
                x.Sku.Value.Contains(input.SearchTerm.ToUpper()));
        }

        if(input.CategoryIds?.Any() == true)
        {
            filtered = true;
            query = query.Where(x => x.Categories.Any(c => input.CategoryIds.Contains(c.CategoryId)));
        }

        if(input.MinPrice is not null)
        {
            filtered = true;
            query = query.Where(x => x.Variants.Any(v => v.Price.Value >= input.MinPrice));
        }

        if(input.MaxPrice is not null)
        {
            filtered = true;
            query = query.Where(x => x.Variants.Any(v => v.Price.Value <= input.MaxPrice));
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        var results = await query
            .Skip(jqueryDataTableParam.Start)
            .Take(jqueryDataTableParam.Length)
            .Select(x => x.ToSearchOutputDto())
            .ToListAsync(cancellationToken);

        return new JqueryDataTableResult
        {
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = results
        };
    }

    public async Task<DetailsOutputDto> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var p = await (from product in _catalogDbContext.Product.Include(p => p.Variants).AsNoTracking()

                       join productCategory in _catalogDbContext.ProductCategory.AsNoTracking()
                          on product.Id equals productCategory.ProductId

                       join category in _catalogDbContext.Category.AsNoTracking()
                          on productCategory.CategoryId equals category.Id

                       where product.Id == id

                       group new { product, category } by product.Id into g

                       select new
                       {
                           Product = g.Select(x => x.product).FirstOrDefault(),
                           Categories = g.Select(x => x.category).ToList()
                       }).AsNoTracking()
                     .FirstOrDefaultAsync(cancellationToken);

        if (p is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        return p.Product.ToDetailsOutputDto(p.Categories);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product.FindAsync(new object[] { id }, cancellationToken);
        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        product.IsDeleted = true;

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(UpdateInputDto input, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Include(p => p.Variants)
            .Include(p => p.Categories)
            .Where(p => p.Id == input.Id)
            .FirstAsync(cancellationToken);

        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        product.SetName(input.Name);
        product.Description = input.Description;
        product.IsActive = input.IsActive;
        product.UpdateCategories(product.Categories.Select(c => c.CategoryId), input.CategoryIds);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> CreateVariantAsync(CreateVariantInputDto input, string contentRootPath, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Include(p => p.Variants.Where(v => v.IsPrimary))
            .Where(p => p.Id == input.ProductId)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }


        var color = await _catalogDbContext.ColorAttribute.FindAsync(new object[] { input.ColorAttributeId }, cancellationToken);
        var size = await _catalogDbContext.SizeAttribute.FindAsync(new object[] { input.SizeAttributeId }, cancellationToken);

        IList<ProductAttribute> attributes = [];
        if (color is not null)
        {
            attributes.Add(new ProductAttribute(AttributeType.Color, color.DisplayName, color.Value, color.Description) { Id = GuidGenerator.CreateSimpleGuid() });
        }
        if (size is not null)
        {
            attributes.Add(new ProductAttribute(AttributeType.Size, size.DisplayName, size.Value, size.Description) { Id = GuidGenerator.CreateSimpleGuid() });
        }

        if (input.IsPrimary)
        {
            var existingPrimaryVariant = product.Variants.FirstOrDefault(v => v.IsPrimary);
            if (existingPrimaryVariant is not null)
            {
                existingPrimaryVariant.IsPrimary = false;
            }
        }

        var newVariant = new ProductVariant(
            GuidGenerator.CreateSimpleGuid(),
            product.Sku,
            product.Name,
            new Money(input.PriceValue),
            attributes,
            input.IsPrimary,
            input.IsActive,
            new StockQuantity(input.StockQuantityValue)
        );

        var saveVariantPhotoFileInputDtos = new List<SaveVariantPhotoFileInputDto>();

        foreach (var photoFile in input.PhotoFiles)
        {
            var newPhoto = new Haskap.DddBase.Domain.Common.File(photoFile.OriginalName);
            newVariant.AddPicture(new ProductPicture(newPhoto, false) { Id = GuidGenerator.CreateSimpleGuid() });

            saveVariantPhotoFileInputDtos.Add(new SaveVariantPhotoFileInputDto
            {
                Content = photoFile.Content,
                Extension = newPhoto.Extension,
                NewName = newPhoto.NewName,
                OriginalName = newPhoto.OriginalName,
            });
        }

        product.AddVariant(newVariant);

        await _catalogDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new VariantCreatedDomainEvent(newVariant.Id, saveVariantPhotoFileInputDtos, contentRootPath), cancellationToken);

        return newVariant.Id;
    }

    public async Task<VariantOutputDto> GetVariantByIdAsync(Guid productId, Guid id, CancellationToken cancellationToken = default)
    {
         var product = await _catalogDbContext.Product
            .Include(p => p.Variants.Where(v => v.Id == id))
            .Where(p => p.Id == productId)
            .FirstAsync(cancellationToken);

        return product.Variants[0].ToOutputDto();
    }

    public async Task UpdateVariantAsync(UpdateVariantInputDto input, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Where(p => p.Id == input.ProductId)
            .Include(p => p.Variants)
            .FirstOrDefaultAsync(cancellationToken);

        if (product is null)
        {
            throw new InvalidOperationException("Product not found.");
        }

        var variant = product.Variants.First(v => v.Id == input.Id);
        variant.SetPrice(new Money(input.PriceValue));
        variant.IsActive = input.IsActive;
        variant.EmptyStock();
        variant.AddStock(input.StockQuantityValue);
        if (input.IsPrimary)
        {
            var existingPrimaryVariant = product.Variants.FirstOrDefault(v => v.IsPrimary && v.Id != input.Id);
            if (existingPrimaryVariant is not null)
            {
                existingPrimaryVariant.IsPrimary = false;
            }
            variant.IsPrimary = true;
        }
        else
        {
            variant.IsPrimary = false;
        }

        var color = await _catalogDbContext.ColorAttribute.FindAsync(new object[] { input.ColorAttributeId }, cancellationToken);
        var size = await _catalogDbContext.SizeAttribute.FindAsync(new object[] { input.SizeAttributeId }, cancellationToken);

        IList<ProductAttribute> attributes = [];
        if (color is not null)
        {
            attributes.Add(new ProductAttribute(AttributeType.Color, color.DisplayName, color.Value, color.Description) { Id = GuidGenerator.CreateSimpleGuid() });
        }
        if (size is not null)
        {
            attributes.Add(new ProductAttribute(AttributeType.Size, size.DisplayName, size.Value, size.Description) { Id = GuidGenerator.CreateSimpleGuid() });
        }
        foreach (var attribute in attributes) 
        {
            variant.AddAttribute(attribute);
        }

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteVariantAsync(DeleteVariantInputDto input, CancellationToken cancellationToken = default)
    {
        var product = await _catalogDbContext.Product
            .Where(p => p.Id == input.ProductId)
            .Include(p => p.Variants.Where(v => v.Id == input.VariantId))
            .FirstOrDefaultAsync(cancellationToken);

        product.Variants[0].IsDeleted = true;

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }
}