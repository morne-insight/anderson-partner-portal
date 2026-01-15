using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.CreateServiceType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateServiceTypeCommandHandler : IRequestHandler<CreateServiceTypeCommand, Guid>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public CreateServiceTypeCommandHandler(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = new ServiceType(
                name: request.Name,
                description: request.Description);

            _serviceTypeRepository.Add(serviceType);
            await _serviceTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return serviceType.Id;
        }
    }
}