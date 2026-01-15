using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.DeleteServiceType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class DeleteServiceTypeCommandHandler : IRequestHandler<DeleteServiceTypeCommand>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public DeleteServiceTypeCommandHandler(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(DeleteServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = await _serviceTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (serviceType is null)
            {
                throw new NotFoundException($"Could not find ServiceType '{request.Id}'");
            }


            _serviceTypeRepository.Remove(serviceType);
        }
    }
}