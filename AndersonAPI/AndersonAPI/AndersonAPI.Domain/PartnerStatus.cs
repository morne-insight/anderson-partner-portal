using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.DomainEnum", Version = "1.0")]

namespace AndersonAPI.Domain
{
    public enum PartnerStatus
    {
        Hired = 1,
        Promoted = 2,
        Terminated = 3
    }
}