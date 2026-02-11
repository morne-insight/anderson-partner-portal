using AndersonAPI.Domain;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Dashboard.GetDashboard
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, DashboardDto>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        [IntentManaged(Mode.Merge)]
        public GetDashboardQueryHandler(
            ICompanyRepository companyRepository,
            IOpportunityRepository opportunityRepository)
        {
                _companyRepository = companyRepository;
                _opportunityRepository = opportunityRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task<DashboardDto> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        {
            var opportunities = await _opportunityRepository
                .FindAllProjectToAsync<DashboardOpportunityDto>(x => x.State == EntityState.Enabled, cancellationToken);

            var partners = await _companyRepository
                .FindAllProjectToAsync<DashboardPartnerDto>(x => x.State == EntityState.Enabled, cancellationToken);

            return new DashboardDto
            {
                Opportunities = opportunities ?? new List<DashboardOpportunityDto>(),
                Partners = partners ?? new List<DashboardPartnerDto>(),
            };
        }
    }
}