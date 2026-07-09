using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Application.Dtos;

public class SearchInputDto
{
    public string? SearchTerm { get; set; } = null;
    public int CurrentPageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 16;
    public OrderBy OrderBy { get; set; } = OrderBy.Default;
    public decimal? MinPriceValue { get; set; } = null;
    public decimal? MaxPriceValue { get; set; } = null;
    public string? CategorySlug { get; set; } = null;
    public IList<string>? FilterAttributes { get; set; } = null;
}
