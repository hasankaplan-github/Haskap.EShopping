namespace Modules.Account.Application.Dtos.Role;

public class UpdateInputDto
{
    public Guid RoleId { get; set; }
    public string NewName { get; set; }
}