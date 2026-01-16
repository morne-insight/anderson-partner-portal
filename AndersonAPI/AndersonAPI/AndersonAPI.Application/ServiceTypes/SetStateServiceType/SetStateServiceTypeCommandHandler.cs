using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.ServiceTypes.SetStateServiceType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateServiceTypeCommandHandler : IRequestHandler<SetStateServiceTypeCommand>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public SetStateServiceTypeCommandHandler(IServiceTypeRepository serviceTypeRepository)
        {
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateServiceTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceType = await _serviceTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (serviceType is null)
            {
                throw new NotFoundException($"Could not find Service Type '{request.Id}'");
            }

            serviceType.SetState(request.State);
        }
    }
}