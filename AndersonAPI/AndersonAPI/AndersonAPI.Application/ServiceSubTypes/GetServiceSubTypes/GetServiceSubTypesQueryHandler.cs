using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.ServiceSubTypes.GetServiceSubTypes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetServiceSubTypesQueryHandler : IRequestHandler<GetServiceSubTypesQuery, List<ServiceSubTypeDto>>
    {
        private readonly IServiceSubTypeRepository _serviceSubTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetServiceSubTypesQueryHandler(IServiceSubTypeRepository serviceSubTypeRepository)
        {
            _serviceSubTypeRepository = serviceSubTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<ServiceSubTypeDto>> Handle(
            GetServiceSubTypesQuery request,
            CancellationToken cancellationToken)
        {
            var serviceSubTypes = await _serviceSubTypeRepository.FindAllProjectToAsync<ServiceSubTypeDto>(cancellationToken);
            return serviceSubTypes;
        }
    }
}