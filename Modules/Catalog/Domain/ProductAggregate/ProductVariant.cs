using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.EShopping.Domain.Common;
using NpgsqlTypes;

namespace Modules.Catalog.Domain.ProductAggregate;

public class ProductVariant : Entity, IAuditable, ISoftDeletable, IIsActive
{
    private static readonly Lock s_stockLock = new();

    public Guid ProductId { get; private set; }
    public Sku Sku { get; private set; }
    public Slug Slug { get; private set; }
    public IReadOnlyList<ProductAttribute> Attributes => _attributes.AsReadOnly();
    private List<ProductAttribute> _attributes = [];
    public IReadOnlyCollection<ProductPicture> Pictures => _pictures.AsReadOnly();
    private List<ProductPicture> _pictures = [];
    public ProductPicture PrimaryPicture => _pictures.Where(x => x.IsPrimary).FirstOrDefault() ?? (_pictures.Any() == true ? _pictures[0] : new(new("variantPhotoPlaceholder.jpg"), true));
    public Money OldPrice { get; private set; }
    public Money Price { get; private set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public StockQuantity StockQuantity { get; private set; }
    public bool IsInStock => StockQuantity.Value > 0;
    public NpgsqlTsVector SearchVectorTurkish { get; set; }
    public Guid? CreatedUserId { get; set; }
    public DateTime? CreatedOnUtc { get; set; }
    public Guid? ModifiedUserId { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public bool IsDeleted { get; set; }

    private ProductVariant()
    { }

    public ProductVariant(
        Guid id,
        Sku productSku,
        string productName,
        Money price,
        IList<ProductAttribute> attributes,
        bool isPrimary,
        bool isActive,
        StockQuantity stockQuantity)
        : base(id)
    {
        Guard.Against.Null(stockQuantity, nameof(stockQuantity));

        SetAttributesAndGenerateSkuAndSlug(productSku, productName, attributes);
        SetPrice(price);
        IsPrimary = isPrimary;
        IsActive = isActive;
        StockQuantity = stockQuantity;
    }

    private void SetAttributesAndGenerateSkuAndSlug(Sku productSku, string productName, IList<ProductAttribute> attributes)
    {
        Guard.Against.Null(productSku, nameof(productSku));
        Guard.Against.Null(attributes, nameof(attributes));
        Guard.Against.InvalidInput(attributes, nameof(attributes), attributes => attributes.All(a => a != null));

        _attributes.AddRange(attributes);

        GenerateSkuAndSlug(productSku.Value, productName);
    }

    public void GenerateSkuAndSlug(string productSkuValue, string productName)
    {
        var attributesText = string.Join("-", _attributes.OrderBy(x => x.AttributeType).Select(a => a.DisplayName));
        Sku = Sku.Generate($"{productSkuValue}_{attributesText}");
        Slug = Slug.Generate($"{productName}--{attributesText}", 50);
    }

    public void AddAttribute(ProductAttribute attribute)
    {
        Guard.Against.Null(attribute, nameof(attribute));

        if (_attributes.Contains(attribute))
        {
            return;
        }

        _attributes.Add(attribute);

        var productSkuValue = Sku.Value.Split('_', 2)[0];
        var productNameSlugValue = Slug.Value.Split("--", 2)[0];
        GenerateSkuAndSlug(productSkuValue, productNameSlugValue);
    }

    public void RemoveAttribute(Guid attributeId)
    {
        var attribute = _attributes.First(a => a.Id == attributeId);
        _attributes.Remove(attribute);

        var productSkuValue = Sku.Value.Split('_', 2)[0];
        var productNameSlugValue = Slug.Value.Split("--", 2)[0];
        GenerateSkuAndSlug(productSkuValue, productNameSlugValue);
    }

    public void AddPicture(ProductPicture picture)
    {
        Guard.Against.Null(picture, nameof(picture));

        _pictures.Add(picture);
    }

    public void RemovePicture(ProductPicture picture)
    {
        Guard.Against.Null(picture, nameof(picture));

        _pictures.Remove(picture);
    }

    public void SetPrice(Money newPrice)
    {
        Guard.Against.Null(newPrice, nameof(newPrice));

        OldPrice = Price is null ? Money.Zero : (Money)Price.ShallowCopy();
        Price = newPrice;
    }

    public void AddStock(int quantity)
    {
        Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Quantity to add must be greater than zero.");

        lock (s_stockLock)
        {
            StockQuantity = new StockQuantity(StockQuantity.Value + quantity);
        }
    }

    public void Sell(int quantity)
    {
        Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Quantity to sell must be greater than zero.");

        lock (s_stockLock)
        {
            Guard.Against.OutOfRange(quantity, nameof(quantity), 1, StockQuantity.Value, "Not enough stock to sell the requested quantity.");

            StockQuantity = new StockQuantity(StockQuantity.Value - quantity);
        }
    }

    public void EmptyStock()
    {
        lock (s_stockLock)
        {
            StockQuantity = new StockQuantity(0);
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
