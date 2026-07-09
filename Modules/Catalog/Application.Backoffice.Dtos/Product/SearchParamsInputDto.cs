namespace Modules.Catalog.Application.Backoffice.Dtos.Product;

public class SearchParamsInputDto
{
    public string? SearchTerm { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public IList<Guid>? CategoryIds { get; set; }
}
