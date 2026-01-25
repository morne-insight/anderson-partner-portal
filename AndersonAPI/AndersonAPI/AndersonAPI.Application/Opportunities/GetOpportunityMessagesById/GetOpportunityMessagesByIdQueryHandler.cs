using System.ComponentModel.DataAnnotations;
using AndersonAPI.Domain.Repositories;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Opportunities.GetOpportunityMessagesById
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetOpportunityMessagesByIdQueryHandler : IRequestHandler<GetOpportunityMessagesByIdQuery, List<OpportunityMessageDto>>
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IMapper _mapper;

        [IntentManaged(Mode.Merge)]
        public GetOpportunityMessagesByIdQueryHandler(IOpportunityRepository opportunityRepository, IMapper mapper)
        {
            _opportunityRepository = opportunityRepository;
            _mapper = mapper;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<List<OpportunityMessageDto>> Handle(
            GetOpportunityMessagesByIdQuery request,
            CancellationToken cancellationToken)
        {
            var opportunity = await _opportunityRepository.FindByIdAsync(request.Id, cancellationToken);
            if (opportunity is null)
            {
                throw new ValidationException($"Opportunity with Id '{request.Id}' could not be found.");
            }

            var messages = opportunity.Messages.MapToOpportunityMessageDtoList(_mapper);
            return messages;
        }
    }
}