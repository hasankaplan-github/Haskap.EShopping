using Haskap.DddBase.Application.Mappings;
using Haskap.EShopping.Application.Mappings;
using Haskap.EShopping.Domain.Common;
using Modules.Catalog.Domain.CategoryAggregate;
using Modules.Catalog.Domain.ColorAttributeAggregate;
using Modules.Catalog.Domain.ProductAggregate;
using Modules.Catalog.Domain.SizeAttributeAggregate;

namespace Modules.Catalog.Application.Backoffice.Mappings;

public static class DtoMappingExtensions
{
    extension(Category category)
    {
        public Dtos.Category.OutputDto ToOutputDto()
        {
            return new()
            {
                Id = category.Id,
                Name = category.Name,
                SlugValue = category.Slug.Value,
                IsActive = category.IsActive,
            };
        }
    }

    extension(Product product)
    {
        public Dtos.Product.SearchOutputDto ToSearchOutputDto()
        {
            return new()
            {
                Id = product.Id,
                Description = product.Description,
                IsActive = product.IsActive,
                Name = product.Name,
                SkuValue = product.Sku.Value,
            };
        }
    }

    extension(SizeAttribute sizeAttribute)
    {
        public Dtos.SizeAttribute.OutputDto ToOutputDto()
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

    extension(ColorAttribute colorAttribute)
    {
        public Dtos.ColorAttribute.OutputDto ToOutputDto()
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

    extension(ProductPicture productPicture)
    {
        public Dtos.Product.ProductPictureDto ToProductPictureDto()
        {
            return new()
            {
                PhotoFile = productPicture.PhotoFile.ToFileOutputDto(),
                IsPrimary = productPicture.IsPrimary
            };
        }
    }

    extension(ProductAttribute productAttribute)
    {
        public Dtos.Product.ProductAttributeOutputDto ToProductAttributeOutputDto()
        {
            return new()
            {
                Id = productAttribute.Id,
                AttributeType = productAttribute.AttributeType,
                DisplayName = productAttribute.DisplayName,
                Value = productAttribute.Value,
                Description = productAttribute.Description
            };
        }
    }

    extension(ProductVariant productVariant)
    {
        public Dtos.Product.VariantOutputDto ToOutputDto()
        {
            return new()
            {
                Id = productVariant.Id,
                ProductId = productVariant.ProductId,
                SkuValue = productVariant.Sku.Value,
                SlugValue = productVariant.Slug.Value,
                OldPrice = productVariant.OldPrice.Value <= productVariant.Price.Value
                    ? Money.Zero.ToMoneyOutputDto()
                    : productVariant.OldPrice.ToMoneyOutputDto(),
                Price = productVariant.Price.ToMoneyOutputDto(),
                IsPrimary = productVariant.IsPrimary,
                IsActive = productVariant.IsActive,
                StockQuantityValue = productVariant.StockQuantity.Value,
                IsInStock = productVariant.IsInStock,
                Attributes = productVariant.Attributes.Select(a => a.ToProductAttributeOutputDto()).ToList().AsReadOnly(),
                Pictures = productVariant.Pictures.Select(p => p.ToProductPictureDto()).ToList().AsReadOnly()
            };
        }
    }

    extension(Product product)
    {
        public Dtos.Product.DetailsOutputDto ToDetailsOutputDto(IList<Category> categories)
        {
            return new()
            {
                Id = product.Id,
                Description = product.Description,
                IsActive = product.IsActive,
                Name = product.Name,
                SkuValue = product.Sku.Value,
                Variants = product.Variants.Select(x => x.ToOutputDto()).ToList().AsReadOnly(),
                Categories = categories.Select(x => x.ToOutputDto()).ToList().AsReadOnly()
            };
        }
    }
}
