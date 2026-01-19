using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetCompanies
{
    public class GetCompaniesQuery : IRequest<List<DirectoryProfileListItem>>, IQuery
    {
        public GetCompaniesQuery()
        {
        }
    }
}