namespace Modules.Shipping.Application.Dtos;

public class ShippingAddressForShippingFeeInputDto
{
    public Guid CityId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid NeighborhoodId { get; set; }
}
