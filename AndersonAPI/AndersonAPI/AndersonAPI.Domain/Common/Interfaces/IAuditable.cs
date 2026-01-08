using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.BasicAuditing.AuditableInterface", Version = "1.0")]

namespace AndersonAPI.Domain.Common.Interfaces
{
    public interface IAuditable
    {
        void SetCreated(Guid createdBy, DateTimeOffset createdDate);
        void SetUpdated(Guid updatedBy, DateTimeOffset updatedDate);
    }
}