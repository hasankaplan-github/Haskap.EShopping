namespace Modules.Catalog.Application.Dtos;

public class SearchOutputDto
{
    public int TotalCount { get; set; }
    public int FilteredCount { get; set; }
    public IReadOnlyList<SearchProductOutputDto> Products { get; set; }
    public FiltersOutputDto Filters { get; set; }
}
