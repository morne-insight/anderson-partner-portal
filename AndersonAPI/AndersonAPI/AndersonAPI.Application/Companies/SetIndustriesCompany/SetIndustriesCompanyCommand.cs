using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.SetIndustriesCompany
{
    public class SetIndustriesCompanyCommand : IRequest, ICommand
    {
        public SetIndustriesCompanyCommand(Guid id, List<Guid> industryIds)
        {
            Id = id;
            IndustryIds = industryIds;
        }

        public Guid Id { get; set; }
        public List<Guid> IndustryIds { get; set; }
    }
}