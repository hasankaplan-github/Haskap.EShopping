using Haskap.DddBase.Domain.Events;

namespace Modules.Account.Domain.AccountAggregate.Events;
public record AccountPermissionsCacheContentUpdatedDomainEvent(Guid AccountId) : DomainEvent;