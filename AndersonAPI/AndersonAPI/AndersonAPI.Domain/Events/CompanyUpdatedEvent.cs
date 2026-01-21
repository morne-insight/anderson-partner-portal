using AndersonAPI.Domain.Common;
using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.DomainEvents.DomainEvent", Version = "1.0")]

namespace AndersonAPI.Domain.Events
{
    public class CompanyUpdatedEvent : DomainEvent
    {
        public CompanyUpdatedEvent(Company company)
        {
            Company = company;
        }

        public Company Company { get; }
    }
}