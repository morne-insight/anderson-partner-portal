using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.ServiceSubTypes.GetServiceSubTypeById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetServiceSubTypeByIdQueryHandler : IRequestHandler<GetServiceSubTypeByIdQuery, ServiceSubTypeDto>
    {
        private readonly IServiceSubTypeRepository _serviceSubTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetServiceSubTypeByIdQueryHandler(IServiceSubTypeRepository serviceSubTypeRepository)
        {
            _serviceSubTypeRepository = serviceSubTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<ServiceSubTypeDto> Handle(
            GetServiceSubTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var serviceSubType = await _serviceSubTypeRepository.FindByIdProjectToAsync<ServiceSubTypeDto>(request.Id, cancellationToken);
            if (serviceSubType is null)
            {
                throw new NotFoundException($"Could not find ServiceSubType '{request.Id}'");
            }
            return serviceSubType;
        }
    }
}