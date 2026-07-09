using Haskap.DddBase.Presentation.CustomAuthorization;
using Modules.Account.Domain.Shared.Consts;

namespace Haskap.EShopping.Ui.MvcWebUi.CustomAuthorization;

public class PermissionProvider : BasePermissionProvider
{
    public override void Define()
    {
        //AddPermission(typeof(Permissions.Gallery), Permissions.Gallery.Editor);

        //AddPermission(typeof(AdminPermissions.ModuleManagement), AdminPermissions.ModuleManagement.ChangeSettings);

        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.Create);
        AddPermission(typeof(AdminPermissions.Account), AdminPermissions.Account.ResetFailedLoginAttemptsAndUnlockWithinSameTenant);

        //AddPermission(typeof(Permissions.Schedule), Permissions.Schedule.ReservationManagement);
        //AddPermission(typeof(Permissions.Schedule), Permissions.Schedule.TourManagement);

        AddPermission(typeof(Permissions.Shop), Permissions.Shop.Owner);
        AddPermission(typeof(Permissions.Shop), Permissions.Shop.AdminPanelAccess);
    }
}