namespace Modules.Account.Application.Dtos.Account;
public class UpdatePermissionsInputDto
{
    public Guid? UserId { get; set; }
    public List<string> CheckedPermissions { get; set; }
    public List<string> UncheckedPermissions { get; set; }
}