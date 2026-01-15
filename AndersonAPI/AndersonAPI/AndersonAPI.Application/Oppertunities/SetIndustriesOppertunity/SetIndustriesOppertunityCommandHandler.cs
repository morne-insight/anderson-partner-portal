using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Oppertunities.SetIndustriesOppertunity
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetIndustriesOppertunityCommandHandler : IRequestHandler<SetIndustriesOppertunityCommand>
    {
        private readonly IOppertunityRepository _oppertunityRepository;
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public SetIndustriesOppertunityCommandHandler(IOppertunityRepository oppertunityRepository, IIndustryRepository industryRepository)
        {
            _oppertunityRepository = oppertunityRepository;
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task Handle(SetIndustriesOppertunityCommand request, CancellationToken cancellationToken)
        {
            var oppertunity = await _oppertunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (oppertunity is null)
            {
                throw new NotFoundException($"Could not find Oppertunity '{request.Id}'");
            }

            var industries = await _industryRepository.FindByIdsAsync(request.IndustryIds.ToArray(), cancellationToken);
            oppertunity.SetIndustries(industries);
        }
    }
}