using Haskap.DddBase.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Shipping.Domain.ShippingAddressAggregate;
using Modules.Shipping.Domain.CityAggregate;
using Modules.Shipping.Domain.DistrictAggregate;
using Modules.Shipping.Domain.NeighborhoodAggregate;
using Modules.Shipping.Domain.ShippingFeeAggregate;

namespace Modules.Shipping.Domain;

public interface IShippingDbContext : IUnitOfWork
{
    DbSet<City> City { get; set; }
    DbSet<District> District { get; set; }
    DbSet<Neighborhood> Neighborhood { get; set; }
    DbSet<ShippingAddress> ShippingAddress { get; set; }
    DbSet<ShippingFee> ShippingFee { get; set; }
}
