using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetServiceTypesOppertunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetServiceTypesOppertunityCommandHandler : IRequestHandler<SetServiceTypesOppertunityCommand>
    {
        private readonly IOppertunityRepository _oppertunityRepository;
        private readonly IServiceTypeRepository _serviceTypeRepository;

        [IntentManaged(Mode.Merge)]
        public SetServiceTypesOppertunityCommandHandler(IOppertunityRepository oppertunityRepository, IServiceTypeRepository serviceTypeRepository)
        {
            _oppertunityRepository = oppertunityRepository;
            _serviceTypeRepository = serviceTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetServiceTypesOppertunityCommand request, CancellationToken cancellationToken)
        {
            var oppertunity = await _oppertunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (oppertunity is null)
            {
                throw new NotFoundException($"Could not find Oppertunity '{request.Id}'");
            }

            var serviceTypes = await _serviceTypeRepository.FindByIdsAsync(request.ServiceTypeIds.ToArray(), cancellationToken);
            oppertunity.SetServiceTypes(serviceTypes);
        }
    }
}