namespace Catalogue.Domain.Abstractions
{
    public abstract class Entity
    {
        int? _requestedHashCode;

        public virtual long Id { get; protected set; }
        public int TenantId { get; set; }
        public long AddedBy { get; set; }
        public long? EditedBy { get; set; }
        public string AddedByName { get; set; }
        public string? EditedByName { get; set; }
        public DateTime AddedOnDate { get; set; }
        public DateTime? EditedOnDate { get; set; }

        private List<DomainEventBase> domainEvents = new List<DomainEventBase>();
        public IReadOnlyCollection<DomainEventBase> DomainEvents => domainEvents.AsReadOnly();

        protected void AddDomainEvent(DomainEventBase eventItem)
        {
            if (eventItem is null)
            {
                throw new ArgumentNullException(nameof(eventItem));
            }

            domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(DomainEventBase eventItem)
        {
            domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents(bool isPostPersistenceEvent)
        {
            domainEvents?.RemoveAll(x => x.IsPostPersistenceEvent == isPostPersistenceEvent);
        }

        public bool IsTransient()
        {
            return Id == default;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Entity))
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id && item.TenantId == TenantId;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ GetType().GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();

        }
    }
}
