namespace Modules.Order.Application.Dtos;

public class CreateInputDto
{
    public Guid IdempotencyKey { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }
}
