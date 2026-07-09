namespace Modules.Account.Domain.Shared.Consts;

public class Permissions
{
    public class Shop
    {
        public const string Owner = $"{nameof(Permissions)}.{nameof(Shop)}.{nameof(Owner)}";
        public const string AdminPanelAccess = $"{nameof(Permissions)}.{nameof(Shop)}.{nameof(AdminPanelAccess)}";
    }
}

public class AdminPermissions
{
    public class Account
    {
        public const string ResetFailedLoginAttemptsAndUnlockWithinSameTenant = $"{nameof(AdminPermissions)}.{nameof(Account)}.{nameof(ResetFailedLoginAttemptsAndUnlockWithinSameTenant)}";
        public const string Create = $"{nameof(AdminPermissions)}.{nameof(Account)}.{nameof(Create)}";
        
    }

    public class ModuleManagement
    {
        public const string ChangeSettings = $"{nameof(AdminPermissions)}.{nameof(ModuleManagement)}.{nameof(ChangeSettings)}";
    }
}