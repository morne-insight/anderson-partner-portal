using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.DomainEnum", Version = "1.0")]

namespace AndersonAPI.Domain
{
    public enum EntityState
    {
        Enabled = 1,
        Disabled = 2,
        Deleted = 3
    }
}