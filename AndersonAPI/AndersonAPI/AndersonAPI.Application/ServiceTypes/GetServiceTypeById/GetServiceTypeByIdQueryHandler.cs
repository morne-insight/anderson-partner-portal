using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.ServiceTypes.GetServiceTypeById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetServiceTypeByIdQueryHandler : IRequestHandler<GetServiceTypeByIdQuery, ServiceTypeDto>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetServiceTypeByIdQueryHandler(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<ServiceTypeDto> Handle(GetServiceTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceType = await _serviceTypeRepository.FindByIdProjectToAsync<ServiceTypeDto>(request.Id, cancellationToken);
            if (serviceType is null)
            {
                throw new NotFoundException($"Could not find ServiceType '{request.Id}'");
            }
            return serviceType;
        }
    }
}