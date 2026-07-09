using Haskap.EShopping.Application.Dtos.Common;

namespace Modules.Catalog.Application.Dtos;

public class ProductVariantDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string SkuValue { get; set; }
    public string SlugValue { get; set; }
    public IReadOnlyList<ProductAttributeOutputDto> Attributes { get; set; }
    public IReadOnlyCollection<ProductPictureDto> Pictures { get; set; }
    public ProductPictureDto PrimaryPicture { get; set; }
    public MoneyOutputDto OldPrice { get; set; }
    public MoneyOutputDto Price { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public int StockQuantityValue { get; set; }
    public bool IsInStock { get; set; }
}
