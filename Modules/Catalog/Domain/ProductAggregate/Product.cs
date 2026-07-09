using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain;
using Modules.Catalog.Domain.CategoryAggregate;
using Modules.Catalog.Domain.Shared.Enums;
using NpgsqlTypes;

namespace Modules.Catalog.Domain.ProductAggregate;

public class Product : AggregateRoot, IAuditable, ISoftDeletable, IIsActive
{
    public string Name { get; private set; }
    public string Description { get; set; }
    public Sku Sku { get; private set; }
    public IReadOnlyList<ProductVariant> Variants => _variants.AsReadOnly();
    private List<ProductVariant> _variants = [];
    public IReadOnlyList<ProductCategory> Categories => _categories.AsReadOnly();
    private List<ProductCategory> _categories = [];
    public IReadOnlyList<ProductAttributeTypeForDetails> AttributeTypePairForDetails => _attributeTypePairForDetails.AsReadOnly();
    private List<ProductAttributeTypeForDetails> _attributeTypePairForDetails = [];
    public bool IsActive { get; set; }
    public NpgsqlTsVector SearchVectorTurkish { get; set; }
    public Guid? CreatedUserId { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public bool IsDeleted { get; set; }

    private Product()
    { }

    public Product(Guid id, string name, string description, bool isActive)
        : base(id)
    {
        SetName(name);
        Description = description;
        IsActive = isActive;
        _attributeTypePairForDetails.AddRange([
            new(AttributeType.Color){Id=GuidGenerator.CreateSimpleGuid()},
            new(AttributeType.Size){Id=GuidGenerator.CreateSimpleGuid()}
        ]);
    }

    public void SetName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        Name = name;
        Sku = Sku.Generate(name);

        foreach (var variant in _variants)
        {
            variant.GenerateSkuAndSlug(Sku.Value, name);
        }
    }

    public void AddCategory(Guid categoryId)
    {
        _categories.Add(new ProductCategory(GuidGenerator.CreateSimpleGuid()) { CategoryId = categoryId });
    }

    public void RemoveCategory(Guid categoryId)
    {
        var toBeRemoved = _categories.Where(x => x.CategoryId == categoryId).First();
        _categories.Remove(toBeRemoved);
    }

    public void AddCategories(IEnumerable<Guid>? checkedCategoryIds)
    {
        if (checkedCategoryIds is null || !checkedCategoryIds.Any())
        {
            return;
        }

        var toBeAdded = checkedCategoryIds
            .Except(_categories.Select(x => x.CategoryId))
            .ToList();

        foreach (var categoryId in toBeAdded)
        {
            AddCategory(categoryId);
        }
    }

    public void RemoveCategories(IEnumerable<Guid>? uncheckedCategoryIds)
    {
        if (uncheckedCategoryIds is null || !uncheckedCategoryIds.Any())
        {
            return;
        }

        if (!_categories.Any())
        {
            return;
        }

        var toBeDeleted = _categories
            .IntersectBy(uncheckedCategoryIds, x => x.CategoryId)
            .ToList();

        foreach (var productCategory in toBeDeleted)
        {
            RemoveCategory(productCategory.CategoryId);
        }
    }

    public void UpdateCategories(IEnumerable<Guid>? uncheckedCategoryIds, IEnumerable<Guid>? checkedCategoryIds)
    {
        RemoveCategories(uncheckedCategoryIds);

        AddCategories(checkedCategoryIds);
    }

    public Guid Copy(Guid id, string name, string description, bool isActive)
    {
        var newProduct = new Product(id, name, description, isActive);

        foreach (var variant in _variants)
        {
            var newVariant = new ProductVariant(
                GuidGenerator.CreateSimpleGuid(),
                newProduct.Sku,
                name,
                variant.Price,
                variant.Attributes.Select(x => (ProductAttribute)x.ShallowCopy()).ToList(),
                variant.IsPrimary,
                variant.IsActive,
                variant.StockQuantity
            );

            newProduct.AddVariant(newVariant);
        }

        foreach (var category in _categories)
        {
            newProduct.AddCategory(category.CategoryId);
        }

        return newProduct.Id;
    }

    public void AddVariant(ProductVariant variant)
    {
        if (!_variants.Any(v => v.Id == variant.Id))
        {
            _variants.Add(variant);
        }
    }
    
    public void RemoveVariant(Guid variantId)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == variantId);
        if (variant is not null)
        {
            _variants.Remove(variant);
        }
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }
}
