namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class DetailsOutputDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string SkuValue { get; set; }
    public bool IsActive { get; set; }
    public IReadOnlyList<VariantOutputDto> Variants { get; set; }
    public IReadOnlyList<Category.OutputDto> Categories { get; set; }
}