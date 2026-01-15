using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.ServiceTypes.GetServiceTypes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetServiceTypesQueryHandler : IRequestHandler<GetServiceTypesQuery, List<ServiceTypeDto>>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetServiceTypesQueryHandler(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<ServiceTypeDto>> Handle(GetServiceTypesQuery request, CancellationToken cancellationToken)
        {
            var serviceTypes = await _serviceTypeRepository.FindAllProjectToAsync<ServiceTypeDto>(cancellationToken);
            return serviceTypes;
        }
    }
}