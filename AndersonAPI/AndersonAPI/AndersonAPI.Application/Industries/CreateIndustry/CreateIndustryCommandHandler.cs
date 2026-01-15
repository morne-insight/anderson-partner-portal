using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Industries.CreateIndustry
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CreateIndustryCommandHandler : IRequestHandler<CreateIndustryCommand, Guid>
    {
        private readonly IIndustryRepository _industryRepository;

        [IntentManaged(Mode.Merge)]
        public CreateIndustryCommandHandler(IIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<Guid> Handle(CreateIndustryCommand request, CancellationToken cancellationToken)
        {
            var industry = new Industry(
                name: request.Name,
                description: request.Description);

            _industryRepository.Add(industry);
            await _industryRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return industry.Id;
        }
    }
}