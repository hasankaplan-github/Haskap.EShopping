using Haskap.DddBase.Domain.Specifications;
using Modules.Catalog.Application.Dtos;
using Modules.Catalog.Domain.Shared.Enums;
using System.Linq.Expressions;

namespace Modules.Catalog.Domain.ProductAggregate.Specifications;

public class AttributeFilterSpecification : Specification<Product>
{
    private readonly IList<ProductAttributeOutputDto> _filterAttributes;

    public AttributeFilterSpecification(IList<ProductAttributeOutputDto> filterAttributes)
    {
        _filterAttributes = filterAttributes;
    }

    public override Expression<Func<Product, bool>> ToExpression()
    {
        Expression<Func<Product, bool>> orExpression = x => false;

        foreach (var filterAttribute in _filterAttributes)
        {
            var valuePrefix = filterAttribute.AttributeType == AttributeType.Color ? "#" : string.Empty;
            var filterAttributeValue = $"{valuePrefix}{filterAttribute.Value}";

            Expression<Func<Product, bool>> otherExpression = x =>
                x.Variants.Any(y =>
                    y.Attributes.Any(z =>
                        filterAttribute.AttributeType == z.AttributeType &&
                        filterAttribute.DisplayName == z.DisplayName &&
                        filterAttributeValue == z.Value));

            orExpression = orExpression.Or(otherExpression);
        }

        return orExpression;
    }
}
