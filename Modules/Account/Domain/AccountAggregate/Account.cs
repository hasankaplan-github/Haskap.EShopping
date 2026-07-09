using Ardalis.GuardClauses;
using Haskap.DddBase.Domain;
using Haskap.DddBase.Domain.Attributes.AuditHistoryLogAttributes;
using Haskap.DddBase.Domain.Common;
using Haskap.DddBase.Domain.Providers;
using Haskap.DddBase.Utilities.Guids;
using Haskap.EShopping.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Account.Domain.AccountAggregate.Exceptions;
using Modules.Account.Domain.RoleAggregate;
using Modules.Account.Domain.Shared.Consts;

namespace Modules.Account.Domain.AccountAggregate;

[AddAuditHistoryLog]
public class Account : AggregateRoot, ISoftDeletable
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string? EmailAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public Credentials Credentials { get; private set; }

    private List<Permission> _permissions = new();
    public IReadOnlyList<Permission> Permissions => _permissions.AsReadOnly();

    private List<AccountRole> _roles = new();
    public IReadOnlyList<AccountRole> Roles => _roles.AsReadOnly();

    public bool IsLocked { get; private set; }
    public LoginAttempt LoginAttempt { get; private set; }

    private List<OpenLogin> _openLogins = new();
    public IReadOnlyList<OpenLogin> OpenLogins=> _openLogins.AsReadOnly();

    public bool IsDeleted { get; set; }

    private Account()
    { }

    public Account(Guid id, string firstName, string lastName, string? emailAddress, string? phoneNumber, Credentials credentials, DbSet<Account> userDbSet)
        : base(id)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        SetCredentials(credentials, userDbSet);
        IsLocked = false;
        LoginAttempt = new();
    }

    public void SignOutFromAllOpenLogins()
    {
        _openLogins.Clear();
    }

    public void SignOut(Guid currentLoginId)
    {
        var currentLogin = _openLogins.Where(x => x.Id == currentLoginId).First();
        _openLogins.Remove(currentLogin);
    }

    public Guid CreateNewLogin(string? remoteIpAddress, string userAgentString)
    {
        var login = new OpenLogin(
            GuidGenerator.CreateSimpleGuid(),
            remoteIpAddress,
            userAgentString);

        _openLogins.Add(login);

        return login.Id;
    }



    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }

   

    public void AddPermission(string permissionName)
    {
        _permissions.Add(new Permission(permissionName));
    }

    public void AddPermissions(IEnumerable<string> checkedPermissions)
    {
        if (checkedPermissions is null || !checkedPermissions.Any())
        {
            return;
        }

        var toBeAdded = checkedPermissions
           .Except(_permissions.Select(x => x.Name))
           .ToList();

        foreach (var permissionName in toBeAdded)
        {
            AddPermission(permissionName);
        }
    }

    public void RemovePermission(string permissionName)
    {
        var toBeRemoved = _permissions.FirstOrDefault(x => x.Name.Equals(permissionName));
        _permissions.Remove(toBeRemoved);
    }

    public void RemovePermission(Permission permission)
    {
        _permissions.Remove(permission);
    }

    public void RemovePermissions(IEnumerable<string> uncheckedPermissions)
    {
        if (uncheckedPermissions is null || !uncheckedPermissions.Any())
        {
            return;
        }

        if (!_permissions.Any())
        {
            return;
        }

        var toBeDeleted = _permissions
            .IntersectBy(uncheckedPermissions, x => x.Name)
            .ToList();

        foreach (var permission in toBeDeleted)
        {
            RemovePermission(permission);
        }
    }

    public void SetFirstName(string firstName)
    {
        Guard.Against.NullOrWhiteSpace(firstName, message: "İsim boş olamaz!");
        Guard.Against.InvalidInput(firstName, nameof(firstName), x => x.Length <= AccountConsts.MaxFirstNameLength, message: "İsim 100 karakterden fazla olamaz!");
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        Guard.Against.NullOrWhiteSpace(lastName, message: "Soyisim boş olamaz!");
        Guard.Against.InvalidInput(lastName, nameof(lastName), x => x.Length <= AccountConsts.MaxFirstNameLength, message: "Soyisim 100 karakterden fazla olamaz!");
        LastName = lastName;
    }

    public void SetCredentials(Credentials credentials, DbSet<Account> userDbSet)
    {
        Guard.Against.Null(credentials);

        var duplicateUserName = userDbSet
            .Where(x => x.Id != Id && x.Credentials.Username.Equals(credentials.Username))
            .Any();

        if (duplicateUserName)
        {
            throw new DuplicateUsernameException();
        }

        Credentials = credentials;
    }

    public void MarkAsDeleted()
    {
        IsDeleted = true;
    }

    public void AddRole(Guid roleId)
    {
        _roles.Add(new AccountRole(GuidGenerator.CreateSimpleGuid()) { RoleId = roleId, AccountId = Id });
    }

    public void AddRoles(IEnumerable<Guid> checkedRoleIds)
    {
        if (checkedRoleIds is null || !checkedRoleIds.Any())
        {
            return;
        }

        var toBeAdded = checkedRoleIds
            .Except(_roles.Select(x => x.RoleId))
            .ToList();

        foreach (var roleId in toBeAdded)
        {
            AddRole(roleId);
        }
    }

    public void RemoveRole(Guid roleId)
    {
        var toBeRemoved = _roles.Where(x => x.RoleId == roleId).First();
        _roles.Remove(toBeRemoved);
    }

    public void RemoveRole(AccountRole role)
    {
        _roles.Remove(role);
    }

    public void RemoveRoles(IEnumerable<Guid> uncheckedRoleIds)
    {
        if (uncheckedRoleIds is null || !uncheckedRoleIds.Any())
        {
            return;
        }

        if (!_roles.Any())
        {
            return;
        }

        var toBeDeleted = _roles
            .IntersectBy(uncheckedRoleIds, x => x.RoleId)
            .ToList();

        foreach (var userRole in toBeDeleted)
        {
            RemoveRole(userRole);
        }
    }

    public void UpdateRoles(IEnumerable<Guid> uncheckedRoleIds, IEnumerable<Guid> checkedRoleIds)
    {
        RemoveRoles(uncheckedRoleIds);

        AddRoles(checkedRoleIds);
    }

    public void UpdatePermissions(IEnumerable<string> uncheckedPermissions, IEnumerable<string> checkedPermissions)
    {
        RemovePermissions(uncheckedPermissions);

        AddPermissions(checkedPermissions);
    }

    public HashSet<string> GetAllPermissions(DbSet<Role> roleDbSet)
    {
        var userPermissions = _permissions.Select(x => x.Name);
        var rolePermissions = roleDbSet
            .Where(x => _roles.Select(y => y.RoleId).Contains(x.Id))
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name);

        return userPermissions.Concat(rolePermissions).ToHashSet();
    }

    public HashSet<string> GetRolePermissions(DbSet<Role> roleDbSet)
    {
        var rolePermissions = roleDbSet
            .Where(x => _roles.Select(y => y.RoleId).Contains(x.Id))
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();

        return rolePermissions;
    }

    public void Update(string firstName, string lastName, string username, string currentClearPassword, IHashProvider hashProvider, DbSet<Account> userDbSet)
    {
        Guard.Against.InvalidInput(Credentials.Password, nameof(currentClearPassword), x => x == new Password(currentClearPassword, Credentials.Password.Salt, hashProvider), exceptionCreator: () => new CurrentPasswordIsWrongException());

        var newCredentials = new Credentials(username, new Password(currentClearPassword, Salt.Generate(), hashProvider));

        SetFirstName(firstName);
        SetLastName(lastName);
        SetCredentials(newCredentials, userDbSet);
    }
}
