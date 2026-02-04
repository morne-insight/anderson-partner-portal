using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.ServiceSubTypes.UpdateServiceSubType
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateServiceSubTypeCommandHandler : IRequestHandler<UpdateServiceSubTypeCommand>
    {
        private readonly IServiceSubTypeRepository _serviceSubTypeRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateServiceSubTypeCommandHandler(IServiceSubTypeRepository serviceSubTypeRepository)
        {
            _serviceSubTypeRepository = serviceSubTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateServiceSubTypeCommand request, CancellationToken cancellationToken)
        {
            var serviceSubType = await _serviceSubTypeRepository.FindByIdAsync(request.Id, cancellationToken);
            if (serviceSubType is null)
            {
                throw new NotFoundException($"Could not find ServiceSubType '{request.Id}'");
            }

            serviceSubType.Update(request.Name, request.Description);
        }
    }
}