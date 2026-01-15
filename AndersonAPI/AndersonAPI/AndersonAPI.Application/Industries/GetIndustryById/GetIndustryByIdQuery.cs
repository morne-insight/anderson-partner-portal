using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Industries.GetIndustryById
{
    public class GetIndustryByIdQuery : IRequest<IndustryDto>, IQuery
    {
        public GetIndustryByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}