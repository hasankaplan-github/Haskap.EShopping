using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Application.Dtos;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SkuValue { get; set; }
    public bool IsActive { get; set; }
    public IReadOnlyList<ProductVariantDto> Variants { get; set; }
    public IReadOnlyList<AttributeType> AttributeTypePairForDetails { get; set; }
}
