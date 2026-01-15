using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.AddContactCompany
{
    public class AddContactCompanyCommand : IRequest, ICommand
    {
        public AddContactCompanyCommand(Guid id,
            string firstName,
            string lastName,
            string? emailAddress,
            string? companyPosition)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            EmailAddress = emailAddress;
            CompanyPosition = companyPosition;
        }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? CompanyPosition { get; set; }
    }
}