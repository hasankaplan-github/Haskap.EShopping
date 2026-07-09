using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Events;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.Account.Application.Contracts.Role;
using Modules.Account.Application.Dtos.Role;
using Modules.Account.Domain;
using Modules.Account.Domain.RoleAggregate.Events;

namespace Modules.Account.Application.Role;
public class RoleService : UseCaseService, IRoleService
{
    private readonly IAccountDbContext _accountsDbContext;
    private readonly IEventPublisher _eventPublisher;

    public RoleService(
        IAccountDbContext accountsDbContext,
        IEventPublisher eventPublisher)
    {
        _accountsDbContext = accountsDbContext;
        _eventPublisher = eventPublisher;
    }

    public async Task DeleteAsync(DeleteInputDto inputDto, CancellationToken cancellationToken)
    {
        var toBeDeleted = await _accountsDbContext.Role
            .Where(x=>x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        _accountsDbContext.Role.Remove(toBeDeleted);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new RolePermissionsCacheContentUpdatedDomainEvent(toBeDeleted.Id), cancellationToken);
    }

    public async Task<List<RoleOutputDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var roles = await _accountsDbContext.Role
            .ToListAsync(cancellationToken);

        var output = roles.Select(x => x.ToRoleOutputDto()).ToList();

        return output;
    }

    public async Task SaveNewAsync(SaveNewInputDto inputDto, CancellationToken cancellationToken)
    {
        var newRole = new Domain.RoleAggregate.Role(
            GuidGenerator.CreateSimpleGuid(),
            inputDto.Name,
            _accountsDbContext.Role);

        await _accountsDbContext.Role.AddAsync(newRole, cancellationToken);
        await _accountsDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto inputDto, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken)
    {
        var query = _accountsDbContext.Role.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (inputDto.Name is not null)
        {
            filtered = true;
            query = query.Where(x => x.Name.ToLower().Contains(inputDto.Name.ToLower()));
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        if (jqueryDataTableParam.Order.Any())
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 0)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Name);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.Name);
        }

        var skip = jqueryDataTableParam.Start;
        var take = jqueryDataTableParam.Length;

        var roles = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        var roleOutputDtos = roles.Select(x => x.ToRoleOutputDto()).ToList();

        return new JqueryDataTableResult
        {
            // this is what datatables wants sending back
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = roleOutputDtos
        };
    }

    public async Task<RoleOutputDto> GetByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _accountsDbContext.Role
            .Where(x => x.Id == roleId)
            .FirstAsync(cancellationToken);

        var output = role.ToRoleOutputDto();

        return output;
    }

    public async Task UpdateAsync(UpdateInputDto inputDto, CancellationToken cancellationToken)
    {
        var role = await _accountsDbContext.Role
            .Where(x => x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        role.SetName(inputDto.NewName, _accountsDbContext.Role);

        await _accountsDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdatePermissionsAsync(UpdatePermissionsInputDto inputDto, CancellationToken cancellationToken)
    {
        var role = await _accountsDbContext.Role
            .Where(x => x.Id == inputDto.RoleId)
            .FirstAsync(cancellationToken);

        role.UpdatePermissions(inputDto.UncheckedPermissions, inputDto.CheckedPermissions);

        await _accountsDbContext.SaveChangesAsync(cancellationToken);

        await _eventPublisher.PublishAsync(new RolePermissionsCacheContentUpdatedDomainEvent(role.Id), cancellationToken);
    }

    public async Task<HashSet<string>> GetPermissionsAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _accountsDbContext.Role
           .Where(x => x.Id == roleId)
           .FirstAsync(cancellationToken);

        return role.Permissions
            .Select(x => x.Name)
            .ToHashSet();
    }
}

