using Haskap.DddBase.Application;
using Haskap.DddBase.Application.Dtos.Common.DataTable;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Application.Backoffice.Contracts;
using Modules.Catalog.Application.Backoffice.Dtos.Category;
using Modules.Catalog.Application.Backoffice.Mappings;
using Modules.Catalog.Domain;
using Modules.Catalog.Domain.CategoryAggregate;

namespace Modules.Catalog.Application.Backoffice;

public class CategoryService : UseCaseService, ICategoryService
{
    private readonly ICatalogDbContext _catalogDbContext;
    private readonly IIsActiveGlobalQueryFilterProvider _isActiveDataFilter;

    public CategoryService(
        ICatalogDbContext catalogDbContext,
        IIsActiveGlobalQueryFilterProvider isActiveDataFilter)
    {
        _catalogDbContext = catalogDbContext;
        _isActiveDataFilter = isActiveDataFilter;
    }

    public async Task CreateAsync(CreateInputDto input, CancellationToken cancellationToken = default)
    {
        var newCategory = new Category(GuidGenerator.CreateSimpleGuid(), input.Name, input.IsActive);
        await _catalogDbContext.Category.AddAsync(newCategory, cancellationToken);
        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _catalogDbContext.Category
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OutputDto>> GetAllActiveCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _catalogDbContext.Category
            .Select(x => x.ToOutputDto())
            .ToListAsync(cancellationToken);

        return categories.AsReadOnly();
    }

    public async Task<IReadOnlyList<OutputDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var _ = _isActiveDataFilter.Disable();

        var categories = await _catalogDbContext.Category
            .Select(x => x.ToOutputDto())
            .ToListAsync(cancellationToken);

        return categories.AsReadOnly();
    }

    public async Task<OutputDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var _ = _isActiveDataFilter.Disable();

        var category = await _catalogDbContext.Category.FindAsync(new object[] { id }, cancellationToken);
        if (category is null)
            throw new InvalidOperationException("Category not found");

        return category.ToOutputDto();
    }

    public async Task<JqueryDataTableResult> SearchAsync(SearchParamsInputDto input, JqueryDataTableParam jqueryDataTableParam, CancellationToken cancellationToken = default)
    {
        var _ = _isActiveDataFilter.Disable();

        var query = _catalogDbContext.Category.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);
        var filteredCount = totalCount;

        var filtered = false;
        if (!string.IsNullOrWhiteSpace(input.SearchTerm))
        {
            filtered = true;
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{input.SearchTerm}%"));
        }

        if(input.IsActive.HasValue)
        {
            filtered = true;
            query = query.Where(x => x.IsActive == input.IsActive);
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
                    query = query.OrderBy(x => x.Name);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Name);
                }
            }
            else if (columnIndex == 2)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.Slug.Value);
                }
                else
                {
                    query = query.OrderByDescending(x => x.Slug.Value);
                }
            }
            else if (columnIndex == 3)
            {
                if (direction == "asc")
                {
                    query = query.OrderBy(x => x.IsActive);
                }
                else
                {
                    query = query.OrderByDescending(x => x.IsActive);
                }
            }
        }
        else
        {
            query = query.OrderBy(x => x.Name);
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
        var _ = _isActiveDataFilter.Disable();

        var category = await _catalogDbContext.Category.FindAsync(new object[] { input.Id }, cancellationToken);
        if (category is null)
            throw new InvalidOperationException("Category not found");

        category.SetName(input.Name);
        category.IsActive = input.IsActive;

        await _catalogDbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<OutputDto>> GetMultipleByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default)
    {
        using var _ = _isActiveDataFilter.Disable();

        var categories = await _catalogDbContext.Category
            .Where(c => ids.Contains(c.Id))
            .Select(c => c.ToOutputDto())
            .ToListAsync(cancellationToken);

        return categories.AsReadOnly();
    }
}
