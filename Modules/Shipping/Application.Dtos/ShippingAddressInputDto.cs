namespace Modules.Shipping.Application.Dtos;

public class ShippingAddressInputDto
{
    public Guid? CityId { get; set; }
    public Guid? DistrictId { get; set; }
    public Guid? NeighborhoodId { get; set; }
    public string? Street { get; set; }
    public string? Postcode { get; set; }
    public string? BuildingNo { get; set; }
    public int? Floor { get; set; }
    public int? ApartmentNo { get; set; }
    public string? AddressLine { get; set; }
}
