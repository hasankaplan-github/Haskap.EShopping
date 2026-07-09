using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Catalog.Domain.CategoryAggregate;
using Modules.Catalog.Domain.ProductAggregate;
using Modules.Catalog.Domain.ColorAttributeAggregate;
using Modules.Catalog.Domain.SizeAttributeAggregate;

namespace Modules.Catalog.Domain;

public interface ICatalogDbContext : IUnitOfWork //IBpmDbContext
{
    DbSet<Product> Product { get; set; }
    DbSet<Category> Category { get; set; }
    DbSet<ProductCategory> ProductCategory { get; set; }
    DbSet<ProductVariant> ProductVariant { get; set; }
    DbSet<ColorAttribute> ColorAttribute { get; set; }
    DbSet<SizeAttribute> SizeAttribute { get; set; }
}
