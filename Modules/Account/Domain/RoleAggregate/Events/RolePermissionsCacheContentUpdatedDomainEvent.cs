using Haskap.DddBase.Domain.Events;

namespace Modules.Account.Domain.RoleAggregate.Events;
public record RolePermissionsCacheContentUpdatedDomainEvent(Guid RoleId) : DomainEvent;