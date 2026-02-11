using AndersonAPI.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.ServiceSubTypes.GetServiceSubTypes
{
    public class GetServiceSubTypesQuery : IRequest<List<ServiceSubTypeDto>>, IQuery
    {
        public GetServiceSubTypesQuery()
        {
        }
    }
}