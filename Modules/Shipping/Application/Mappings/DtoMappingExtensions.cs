using Modules.Shipping.Application.Dtos;
using Modules.Shipping.Domain.ShippingAddressAggregate;
using Modules.Shipping.Domain.CityAggregate;
using Modules.Shipping.Domain.DistrictAggregate;
using Modules.Shipping.Domain.NeighborhoodAggregate;

namespace Modules.Shipping.Application.Mappings;

public static class DtoMappingExtensions
{
    extension(ShippingAddress shippingAddress)
    {
        public ShippingAddressOutputDto ToShippingAddressOutputDto()
        {
            return new()
            {
                Id = shippingAddress.Id,
                OwnerAccountId = shippingAddress.OwnerAccountId,
                CityId = shippingAddress.CityId,
                CityName = shippingAddress.City.Name,
                DistrictId = shippingAddress.DistrictId,
                DistrictName = shippingAddress.District.Name,
                NeighborhoodId = shippingAddress.NeighborhoodId,
                NeighborhoodName = shippingAddress.Neighborhood.Name,
                Street = shippingAddress.Street,
                Postcode = shippingAddress.Postcode,
                BuildingNo = shippingAddress.BuildingNo,
                Floor = shippingAddress.Floor,
                ApartmentNo = shippingAddress.ApartmentNo,
                AddressLine = shippingAddress.AddressLine,
            };
        }
    }

    extension(Neighborhood neighborhood)
    {
        public NeighborhoodOutputDto ToNeighborhoodOutputDto()
        {
            return new()
            {
                Id = neighborhood.Id,
                Name = neighborhood.Name
            };
        }
    }

    extension(District district)
    {
        public DistrictOutputDto ToDistrictOutputDto()
        {
            return new()
            {
                Id = district.Id,
                Name = district.Name,
            };
        }
    }

    extension(City city)
    {
        public CityOutputDto ToCityOutputDto()
        {
            return new()
            {
                Id = city.Id,
                Name = city.Name,
            };
        }
    }
}