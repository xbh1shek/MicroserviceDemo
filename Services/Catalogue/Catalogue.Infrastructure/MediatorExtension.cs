
using Catalogue.Domain.Abstractions;
using Catalogue.Infrastructure.Database;
using MediatR;

namespace Catalogue.Infrastructure
{
    static class MediatorExtension
    {
        public static async Task DispatchPrePersistenceDomainEventsAsync(this IMediator mediator, CatalogueDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .Where(d => !d.IsPostPersistenceEvent)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents(false));

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }

        public static async Task DispatchPostPersistenceDomainEventsAsync(this IMediator mediator, CatalogueDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                 .Where(d => d.IsPostPersistenceEvent)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents(true));

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
