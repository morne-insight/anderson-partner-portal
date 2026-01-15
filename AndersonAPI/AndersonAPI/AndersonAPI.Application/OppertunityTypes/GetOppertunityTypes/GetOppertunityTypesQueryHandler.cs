using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.OppertunityTypes.GetOppertunityTypes
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOppertunityTypesQueryHandler : IRequestHandler<GetOppertunityTypesQuery, List<OppertunityTypeDto>>
    {
        private readonly IOppertunityTypeRepository _oppertunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetOppertunityTypesQueryHandler(IOppertunityTypeRepository oppertunityTypeRepository)
        {
            _oppertunityTypeRepository = oppertunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<List<OppertunityTypeDto>> Handle(
            GetOppertunityTypesQuery request,
            CancellationToken cancellationToken)
        {
            var oppertunityTypes = await _oppertunityTypeRepository.FindAllProjectToAsync<OppertunityTypeDto>(cancellationToken);
            return oppertunityTypes;
        }
    }
}