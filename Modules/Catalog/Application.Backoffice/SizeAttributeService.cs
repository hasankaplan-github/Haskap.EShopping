using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.SizeAttribute;
using Modules.Catalog.Application.Backoffice.Mappings;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.SizeAttributeAggregate;

namespace Modules.Catalog.Application.Backoffice;

public class SizeAttributeService : UseCaseService, ISizeAttributeService
{
    private readonly ICatalogDbContext _catalogDbContext;

    public SizeAttributeService(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task CreateAsync(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        var newSizeAttribute = new SizeAttribute(GuidGenerator.CreateSimpleGuid(), input.DisplayName, input.Value, input.Description);
        await _catalogDbContext.SizeAttribute.AddAsync(newSizeAttribute, cancellationToken);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _catalogDbContext.SizeAttribute
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<OutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sizeAttribute = await _catalogDbContext.SizeAttribute.FindAsync(new object[] { id }, cancellationToken);
        if (sizeAttribute is null)
            throw new InvalidOperationException("Product size attribute not found");

        return sizeAttribute.ToOutputDto();
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var query = _catalogDbContext.SizeAttribute.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (!string.IsNullOrWhiteSpace(input.SearchTerm))
        {
            filtered = true;
            query = query.Where(x => 
                EF.Functions.ILike(x.DisplayName, $"%{input.SearchTerm}%") ||
                EF.Functions.ILike(x.Value, $"%{input.SearchTerm}%") ||
                EF.Functions.ILike(x.Description ?? string.Empty, $"%{input.SearchTerm}%"));
        }

        if (filtered)
        {
            filteredCount = await query.CountAsync(cancellationToken);
        }

        if (jqueryDataTableParam.Order?.Any() == true)
        {
            var direction = jqueryDataTableParam.Order[0].Dir;
            var columnIndex = jqueryDataTableParam.Order[0].Column;

            if (columnIndex == 1)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.DisplayName);
                }
                else
                {
                    query = query.OrderByDescending(x => x.DisplayName);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Value);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Value);
                }
            }
            else if (columnIndex == 3)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Description);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Description);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.DisplayName);
        }

        var data = await query
            .Skip(jqueryDataTableParam.Start)
            .Take(jqueryDataTableParam.Length)
            .Select(x => x.ToOutputDto())
            .ToListAsync(cancellationToken);

        return new JqueryDataTableResult
        {
            draw = jqueryDataTableParam.Draw,
            recordsTotal = totalCount,
            recordsFiltered = filteredCount,
            data = data
        };
    }

    public async Task UpdateAsync(UpdateInputDto input, CancellationToken cancellationToken = default)
    {
        var sizeAttribute = await _catalogDbContext.SizeAttribute.FindAsync(new object[] { input.Id }, cancellationToken);
        if (sizeAttribute is null)
            throw new InvalidOperationException("Product size attribute not found");

        sizeAttribute.SetDisplayName(input.DisplayName);
        sizeAttribute.SetValue(input.Value);
        sizeAttribute.Description = input.Description;

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<OutputDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _catalogDbContext.SizeAttribute
            .OrderBy(x => x.DisplayName)
            .Select(x => x.ToOutputDto())
            .ToListAsync(cancellationToken);
    }
}
