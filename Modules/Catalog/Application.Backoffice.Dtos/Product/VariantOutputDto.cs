using Haskap.EShopping.Application.Dtos.Common;

namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class VariantOutputDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string SkuValue { get; set; }
    public string SlugValue { get; set; }
    public IReadOnlyList<ProductAttributeOutputDto> Attributes { get; set; }
    public IReadOnlyCollection<ProductPictureDto> Pictures { get; set; }
    public MoneyOutputDto OldPrice { get; set; }
    public MoneyOutputDto Price { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsActive { get; set; }
    public int StockQuantityValue { get; set; }
    public bool IsInStock { get; set; }
}
