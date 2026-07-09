using Haskap.DddBase.Application.Mappings;
using Haskap.EShopping.Application.Mappings;
using Haskap.EShopping.Domain.Common;
using Modules.Catalog.Application.Dtos;
using Modules.Catalog.Domain.ProductAggregate;
using Modules.Catalog.Domain.ColorAttributeAggregate;
using Modules.Catalog.Domain.SizeAttributeAggregate;
using Modules.Catalog.Domain.Shared.Enums;

namespace Modules.Catalog.Application.Mappings;

public static class DtoMappingExtensions
{
    extension(Product product)
    {
        public ProductDto ToProductDto()
        {
            return new()
            {
                Id = product.Id,
                Description = product.Description,
                IsActive = product.IsActive,
                Name = product.Name,
                SkuValue = product.Sku.Value,
                Variants = product.Variants.Select(v => v.ToProductVariantDto()).ToList().AsReadOnly(),
                AttributeTypePairForDetails = product.AttributeTypePairForDetails.Select(x => x.AttributeType).ToList().AsReadOnly(),
            };
        }
    }

    extension(ProductVariant productVariant)
    {
        public ProductVariantDto ToProductVariantDto()
        {
            return new()
            {
                Id = productVariant.Id,
                IsActive = productVariant.IsActive,
                IsInStock = productVariant.IsInStock,
                IsPrimary = productVariant.IsPrimary,
                Attributes = productVariant.Attributes.Select(a => a.ToProductAttributeOutputDto()).ToList().AsReadOnly(),
                OldPrice = productVariant.OldPrice.Value <= productVariant.Price.Value
                    ? Money.Zero.ToMoneyOutputDto()
                    : productVariant.OldPrice.ToMoneyOutputDto(),
                Pictures = productVariant.Pictures.Select(p => p.ToProductPictureDto()).ToList().AsReadOnly(),
                PrimaryPicture = productVariant.PrimaryPicture.ToProductPictureDto(),
                Price = productVariant.Price.ToMoneyOutputDto(),
                ProductId = productVariant.ProductId,
                SkuValue = productVariant.Sku.Value,
                SlugValue = productVariant.Slug.Value,
                StockQuantityValue = productVariant.StockQuantity.Value
            };
        }
    }

    extension(ProductPicture productPicture)
    {
        public ProductPictureDto ToProductPictureDto()
        {
            return new()
            {
                PhotoFile = productPicture.PhotoFile.ToFileOutputDto(),
                IsPrimary = productPicture.IsPrimary
            };
        }
    }

    extension(ColorAttribute colorAttribute)
    {
        public ColorAttributeOutputDto ToColorAttributeOutputDto()
        {
            return new()
            {
                Id = colorAttribute.Id,
                DisplayName = colorAttribute.DisplayName,
                Value = colorAttribute.Value,
                Description = colorAttribute.Description
            };
        }
    }

    extension(SizeAttribute sizeAttribute)
    {
        public SizeAttributeOutputDto ToSizeAttributeOutputDto()
        {
            return new()
            {
                Id = sizeAttribute.Id,
                DisplayName = sizeAttribute.DisplayName,
                Value = sizeAttribute.Value,
                Description = sizeAttribute.Description
            };
        }
    }

    extension(ProductAttribute productAttribute)
    {
        public ProductAttributeOutputDto ToProductAttributeOutputDto()
        {
            return new()
            {
                Id = productAttribute.Id,
                AttributeType = productAttribute.AttributeType,
                DisplayName = productAttribute.DisplayName,
                Value = productAttribute.Value
            };
        }
    }

    extension(IList<string>? filterAttributes)
    {
        public IList<ProductAttributeOutputDto> ToProductAttributeOutputDtos()
        {
            var attributes = filterAttributes?.Select(x =>
            {
                if (x is null)
                {
                    return null;
                }

                var attrTemp = x.Split('|');

                if (attrTemp.Length != 3)
                {
                    return null;
                }

                if (!Enum.TryParse<AttributeType>(attrTemp[0], out var attributeType))
                {
                    return null;
                }

                return new ProductAttributeOutputDto
                {
                    AttributeType = attributeType,
                    DisplayName = attrTemp[1],
                    Value = attrTemp[2]
                };
            }).OfType<ProductAttributeOutputDto>()
            .ToList() ?? [];

            return attributes;
        }
    }
}
