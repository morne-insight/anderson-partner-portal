using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.DomainEnum", Version = "1.0")]

namespace AndersonAPI.Domain
{
    public enum OpportunityStatus
    {
        Open = 1,
        Closed = 2
    }
}