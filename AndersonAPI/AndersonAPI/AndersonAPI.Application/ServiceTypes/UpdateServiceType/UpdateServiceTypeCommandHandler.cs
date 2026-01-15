using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.UpdateServiceType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateServiceTypeCommandHandler : IRequestHandler<UpdateServiceTypeCommand>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateServiceTypeCommandHandler(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = await _serviceTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (serviceType is null)
            {
                throw new NotFoundException($"Could not find ServiceType '{request.Id}'");
            }

            serviceType.Update(request.Name, request.Description);
        }
    }
}