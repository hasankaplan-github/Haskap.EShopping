namespace Modules.Account.Application.Dtos.Role;
public class UpdatePermissionsInputDto
{
    public Guid RoleId { get; set; }
    public List<string> CheckedPermissions { get; set; }
    public List<string> UncheckedPermissions { get; set; }
}