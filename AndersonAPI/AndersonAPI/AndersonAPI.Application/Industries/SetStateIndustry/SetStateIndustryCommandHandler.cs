using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Industries.SetStateIndustry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class SetStateIndustryCommandHandler : IRequestHandler<SetStateIndustryCommand>
    {
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public SetStateIndustryCommandHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(SetStateIndustryCommand request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (industry is null)
            {
                throw new NotFoundException($"Could not find Industry '{request.Id}'");
            }

            industry.SetState(request.State);
        }
    }
}