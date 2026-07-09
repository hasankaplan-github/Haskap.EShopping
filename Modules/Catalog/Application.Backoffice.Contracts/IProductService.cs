using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Modules.Catalog.Application.Backoffice.Dtos.Product;

namespace Modules.Catalog.Application.Backoffice.Contracts;

public interface IProductService : IUseCaseService
{
    Task<SearchOutputDto> CreateAsync(CreateInputDto input, CancellationToken cancellationToken = default);
    Task AddStockAsync(AddStockInputDto input, CancellationToken cancellationToken = default);
    Task EmptyStockAsync(EmptyStockInputDto input, CancellationToken cancellationToken = default);
    Task<Guid> CopyAsync(CopyInputDto input, CancellationToken cancellationToken = default);
    Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default);
    Task<DetailsOutputDto> GetDetailsByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateInputDto input, CancellationToken cancellationToken = default);


    Task<Guid> CreateVariantAsync(CreateVariantInputDto input, string contentRootPath, CancellationToken cancellationToken = default);
    Task<VariantOutputDto> GetVariantByIdAsync(Guid productId, Guid id, CancellationToken cancellationToken = default);
    Task UpdateVariantAsync(UpdateVariantInputDto input, CancellationToken cancellationToken = default);
    Task DeleteVariantAsync(DeleteVariantInputDto input, CancellationToken cancellationToken = default);
}
