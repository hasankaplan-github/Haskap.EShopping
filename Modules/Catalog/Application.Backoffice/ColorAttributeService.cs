using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.ColorAttribute;
using Modules.Catalog.Application.Backoffice.Mappings;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.ColorAttributeAggregate;

namespace Modules.Catalog.Application.Backoffice;

public class ColorAttributeService : UseCaseService, IColorAttributeService
{
    private readonly ICatalogDbContext _catalogDbContext;

    public ColorAttributeService(ICatalogDbContext catalogDbContext)
    {
        _catalogDbContext = catalogDbContext;
    }

    public async Task CreateAsync(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        var newColorAttribute = new ColorAttribute(GuidGenerator.CreateSimpleGuid(), input.DisplayName, input.Value, input.Description);
        await _catalogDbContext.ColorAttribute.AddAsync(newColorAttribute, cancellationToken);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _catalogDbContext.ColorAttribute
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<OutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var colorAttribute = await _catalogDbContext.ColorAttribute.FindAsync(new object[] { id }, cancellationToken);
        if (colorAttribute is null)
            throw new InvalidOperationException("Product color attribute not found");

        return colorAttribute.ToOutputDto();
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var query = _catalogDbContext.ColorAttribute.AsQueryable();

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
        var colorAttribute = await _catalogDbContext.ColorAttribute.FindAsync(new object[] { input.Id }, cancellationToken);
        if (colorAttribute is null)
            throw new InvalidOperationException("Product color attribute not found");

        colorAttribute.SetDisplayName(input.DisplayName);
        colorAttribute.SetValue(input.Value);
        colorAttribute.Description = input.Description;

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<OutputDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _catalogDbContext.ColorAttribute
            .OrderBy(x => x.DisplayName)
            .Select(x => x.ToOutputDto())
            .ToListAsync(cancellationToken);
    }
}
