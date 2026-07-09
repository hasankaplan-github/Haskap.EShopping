using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Application.Dtos;

public class ProductDetailsOutputDto
{
    public ProductDto Product { get; set; }
    public ProductVariantDto SelectedVariant { get; set; }
    public IReadOnlyDictionary<string, string> Slugs => Product.Variants
        .ToDictionary(x => string.Join(',', x.Attributes.OrderBy(x => x.AttributeType).Select(y => y.DisplayName)), x => x.SlugValue);
    public IReadOnlyDictionary<AttributeType, HashSet<ProductAttributeOutputDto>> ValidProductAttributes
    {
        get
        {
            var validProductAttributes = new Dictionary<AttributeType, HashSet<ProductAttributeOutputDto>>();

            if (Product.AttributeTypePairForDetails.Count == 0)
            {
                return validProductAttributes;
            }

            if (Product.AttributeTypePairForDetails.Count == 1)
            {
                var firstAttributeType = Product.AttributeTypePairForDetails[0];

                // TODO: belki hashset comparer eklenebilir.
                validProductAttributes.Add(firstAttributeType, new HashSet<ProductAttributeOutputDto>(
                    Product.Variants
                        .SelectMany(x => x.Attributes)
                        .Where(x=>x.AttributeType == firstAttributeType)));

                return validProductAttributes;
            }

            foreach (var attributePairIndexes in new List<(int FirstIndex, int SecondIndex)> { (0, 1), (1, 0) })
            {
                var firstAttributeType = Product.AttributeTypePairForDetails[attributePairIndexes.FirstIndex];
                var secondAttributeType = Product.AttributeTypePairForDetails[attributePairIndexes.SecondIndex];

                validProductAttributes.Add(secondAttributeType, new HashSet<ProductAttributeOutputDto>());

                var selectedVariantFirstAttribute = SelectedVariant.Attributes.Where(x => x.AttributeType == firstAttributeType).First();
                foreach (var availableAttributes in Product.Variants.Select(x => x.Attributes))
                {
                    if (availableAttributes.Any(x =>
                        x.AttributeType == selectedVariantFirstAttribute.AttributeType &&
                        x.DisplayName == selectedVariantFirstAttribute.DisplayName &&
                        x.Value == selectedVariantFirstAttribute.Value))
                    {
                        var attributes = validProductAttributes.GetValueOrDefault(secondAttributeType)!;
                        var attribute = availableAttributes.Where(x => x.AttributeType == secondAttributeType).FirstOrDefault();
                        if (attribute is not null)
                        {
                            // TODO: belki hashset comparer eklenebilir.
                            attributes.Add(attribute);
                        }
                    }
                }
            }

            return validProductAttributes;
        }
    }
}
