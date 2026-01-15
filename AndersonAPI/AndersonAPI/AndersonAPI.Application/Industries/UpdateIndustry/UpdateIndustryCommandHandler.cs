using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Industries.UpdateIndustry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateIndustryCommandHandler : IRequestHandler<UpdateIndustryCommand>
    {
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public UpdateIndustryCommandHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task Handle(UpdateIndustryCommand request, CancellationToken cancellationToken)
        {
            var industry = await _industryRepository.FindByIdAsync(request.Id, cancellationToken);
            if (industry is null)
            {
                throw new NotFoundException($"Could not find Industry '{request.Id}'");
            }

            industry.Update(request.Name, request.Description);
        }
    }
}