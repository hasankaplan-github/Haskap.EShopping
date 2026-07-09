using Haskap.DddBase.Application.Contracts;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Modules.Account.Application.Dtos.Account;
using Modules.Account.Application.Dtos.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Account.Application.Contracts.Account;
public interface IAccountService : IUseCaseService
{
    Task<HashSet<string>> GetAllPermissionsAsync(GetAllPermissionsInputDto input, CancellationToken cancellationToken = default);
    Task<LoginOutputDto> LoginAsync(LoginInputDto inputDto, CancellationToken cancellationToken = default);
    Task<OpenLoginOutputDto?> GetOpenLoginAsync(Guid accountId, Guid loginId, CancellationToken cancellationToken);
    Task SetLoginLastSeenDateTimeAsync(Guid accountId, Guid loginId, DateTime utcCurrentLastSeen, CancellationToken cancellationToken);
    Task<CreateOutputDto> CreateAsync(CreateInputDto input, CancellationToken cancellationToken);
    Task SignOutOfOpenLoginAsync(Guid accountId, Guid openLoginId, CancellationToken cancellationToken);
    Task<IReadOnlyList<OpenLoginOutputDto>> GetOpenLoginsOfUserAsync(Guid? tenantId, Guid userId, CancellationToken cancellationToken);
    Task<AccountOutputDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    Task SignOutAsync(CancellationToken cancellationToken);
    Task ChangePasswordAsync(ChangePasswordInputDto inputDto, CancellationToken cancellationToken = default);
    Task UpdateAsync(Dtos.Account.UpdateInputDto inputDto, CancellationToken cancellationToken);
    Task<HashSet<string>> GetUserPermissionsAsync(GetUserPermissionsInputDto inputDto, CancellationToken cancellationToken = default);
    Task UpdatePermissionsAsync(Dtos.Account.UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken);
    Task<List<RoleOutputDto>> GetRolesAsync(GetRolesInputDto inputDto, CancellationToken cancellationToken);
    Task UpdateRolesAsync(Dtos.Account.UpdateRolesInputDto inputDto, CancellationToken cancellationToken);
    Task ResetFailedLoginAttemptsAndUnlockAsync(ResetFailedLoginAttemptsAndUnlockInputDto inputDto, CancellationToken cancellationToken);
    Task<JqueryDataTableResult> SearchAsync(Dtos.Account.SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken);
    Task<List<AccountOutputDto>> GetUsersByIdAsync(List<Guid> userIds, CancellationToken cancellationToken);
}
