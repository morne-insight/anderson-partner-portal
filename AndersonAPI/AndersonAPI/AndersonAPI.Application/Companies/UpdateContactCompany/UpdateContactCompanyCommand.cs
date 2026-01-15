using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.UpdateContactCompany
{
    public class UpdateContactCompanyCommand : IRequest, ICommand
    {
        public UpdateContactCompanyCommand(Guid id,
            Guid contactId,
            string firstName,
            string lastName,
            string? emailAddress,
            string? companyPosition)
        {
            Id = id;
            ContactId = contactId;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            CompanyPosition = companyPosition;
        }

        public Guid Id { get; set; }
        public Guid ContactId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? CompanyPosition { get; set; }
    }
}