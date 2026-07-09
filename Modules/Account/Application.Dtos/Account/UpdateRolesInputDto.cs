namespace Modules.Account.Application.Dtos.Account;
public class UpdateRolesInputDto
{
    public Guid? UserId { get; set; }
    public List<Guid>? CheckedRoles { get; set; }
    public List<Guid>? UncheckedRoles { get; set; }
}