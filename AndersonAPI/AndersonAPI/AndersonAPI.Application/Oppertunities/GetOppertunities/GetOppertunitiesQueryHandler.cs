using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.GetOppertunities
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOppertunitiesQueryHandler : IRequestHandler<GetOppertunitiesQuery, List<OppertunityListItemDto>>
    {
        private readonly IOppertunityRepository _oppertunityRepository;

        [IntentManaged(Mode.Merge)]
        public GetOppertunitiesQueryHandler(IOppertunityRepository oppertunityRepository)
        {
            _oppertunityRepository = oppertunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<OppertunityListItemDto>> Handle(
            GetOppertunitiesQuery request,
            CancellationToken cancellationToken)
        {
            var oppertunities = await _oppertunityRepository.FindAllProjectToAsync<OppertunityListItemDto>(cancellationToken);
            return oppertunities;
        }
    }
}