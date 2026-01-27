using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.DomainEnum", Version = "1.0")]

namespace AndersonAPI.Domain
{
    public enum EmailTemplateType
    {
        AccountConfirmation = 1,
        ResetPassword = 2,
        ConnectWithPartner = 3
    }
}