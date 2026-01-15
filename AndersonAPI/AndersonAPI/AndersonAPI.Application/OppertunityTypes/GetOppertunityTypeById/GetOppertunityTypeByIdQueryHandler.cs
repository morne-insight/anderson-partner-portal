using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.OppertunityTypes.GetOppertunityTypeById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOppertunityTypeByIdQueryHandler : IRequestHandler<GetOppertunityTypeByIdQuery, OppertunityTypeDto>
    {
        private readonly IOppertunityTypeRepository _oppertunityTypeRepository;

        [IntentManaged(Mode.Merge)]
        public GetOppertunityTypeByIdQueryHandler(IOppertunityTypeRepository oppertunityTypeRepository)
        {
            _oppertunityTypeRepository = oppertunityTypeRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public async Task<OppertunityTypeDto> Handle(
            GetOppertunityTypeByIdQuery request,
            CancellationToken cancellationToken)
        {
            var oppertunityType = await _oppertunityTypeRepository.FindByIdProjectToAsync<OppertunityTypeDto>(request.Id, cancellationToken);
            if (oppertunityType is null)
            {
                throw new NotFoundException($"Could not find OppertunityType '{request.Id}'");
            }
            return oppertunityType;
        }
    }
}