using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.UpdateOppertunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateOppertunityCommandHandler : IRequestHandler<UpdateOppertunityCommand>
    {
        private readonly IOppertunityRepository _oppertunityRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateOppertunityCommandHandler(IOppertunityRepository oppertunityRepository)
        {
            _oppertunityRepository = oppertunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateOppertunityCommand request, CancellationToken cancellationToken)
        {
            var oppertunity = await _oppertunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (oppertunity is null)
            {
                throw new NotFoundException($"Could not find Oppertunity '{request.Id}'");
            }

            oppertunity.Update(
                request.Title,
                request.ShortDescription,
                request.FullDescription,
                request.Deadline,
                request.OppertunityTypeId,
                request.CountryId);
        }
    }
}