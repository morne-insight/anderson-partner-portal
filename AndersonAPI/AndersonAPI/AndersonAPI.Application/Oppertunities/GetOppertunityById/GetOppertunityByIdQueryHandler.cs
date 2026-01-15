using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Oppertunities.GetOppertunityById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOppertunityByIdQueryHandler : IRequestHandler<GetOppertunityByIdQuery, OppertunityDto>
    {
        private readonly IOppertunityRepository _oppertunityRepository;

        [IntentManaged(Mode.Merge)]
        public GetOppertunityByIdQueryHandler(IOppertunityRepository oppertunityRepository)
        {
            _oppertunityRepository = oppertunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<OppertunityDto> Handle(GetOppertunityByIdQuery request, CancellationToken cancellationToken)
        {
            var oppertunity = await _oppertunityRepository.FindByIdProjectToAsync<OppertunityDto>(request.Id, cancellationToken);
            if (oppertunity is null)
            {
                throw new NotFoundException($"Could not find Oppertunity '{request.Id}'");
            }
            return oppertunity;
        }
    }
}