using MediatR;

namespace Catalogue.Domain.Abstractions
{
    public abstract class DomainEventBase : INotification
    {
        public bool IsPostPersistenceEvent { get; protected set; } = false;
    }
}
