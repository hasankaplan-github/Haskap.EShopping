using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Modules.Catalog.Application.Backoffice.Dtos.Category;

namespace Modules.Catalog.Application.Backoffice.Contracts;

public interface ICategoryService : IUseCaseService
{
    Task CreateAsync(CreateInputDto input, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OutputDto>> GetAllActiveCategoriesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OutputDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<OutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdateInputDto input, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OutputDto>> GetMultipleByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default);
}
