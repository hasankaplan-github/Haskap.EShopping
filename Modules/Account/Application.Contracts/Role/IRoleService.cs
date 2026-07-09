using Modules.Account.Application.Dtos.Role;
using Haskap.DddBase.Application.Dtos.Common.DataTable;

namespace Modules.Account.Application.Contracts.Role;
public interface IRoleService
{
    Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken);
    Task<List<RoleOutputDto>> GetAllAsync(CancellationToken cancellationToken);
    Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken);
    Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken);
    Task<RoleOutputDto> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
    Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken);
    Task UpdatePermissionsAsync(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken);
    Task<HashSet<string>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken);
}
