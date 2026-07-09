namespace Modules.Catalog.Application.Dtos;

public class SearchProductOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SkuValue { get; set; }
    public bool IsActive { get; set; }
    public ProductVariantDto PrimaryVariant { get; set; }
}
