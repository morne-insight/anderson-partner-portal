using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.ServiceSubTypes.CreateServiceSubType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateServiceSubTypeCommandHandler : IRequestHandler<CreateServiceSubTypeCommand, Guid>
    {
        private readonly IServiceSubTypeRepository _serviceSubTypeRepository;

        [IntentManaged(Mode.Merge)]
        public CreateServiceSubTypeCommandHandler(IServiceSubTypeRepository serviceSubTypeRepository)
        {
            _serviceSubTypeRepository = serviceSubTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateServiceSubTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceSubType = new ServiceSubType(
                serviceTypeId: request.ServiceTypeId,
                name: request.Name,
                description: request.Description);

            _serviceSubTypeRepository.Add(serviceSubType);
            await _serviceSubTypeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return serviceSubType.Id;
        }
    }
}