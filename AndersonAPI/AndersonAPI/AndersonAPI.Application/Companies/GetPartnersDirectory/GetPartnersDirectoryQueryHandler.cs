using AndersonAPI.Application.Common.Pagination;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using LinqKit;
using MediatR;
using static System.Linq.Dynamic.Core.DynamicQueryableExtensions;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetPartnersDirectory
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetPartnersDirectoryQueryHandler : IRequestHandler<GetPartnersDirectoryQuery, PagedResult<PartnerProfileListItem>>
    {
        private readonly ICompanyRepository _companyRepository;

        [IntentManaged(Mode.Merge)]
        public GetPartnersDirectoryQueryHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<PagedResult<PartnerProfileListItem>> Handle(
            GetPartnersDirectoryQuery request,
            CancellationToken cancellationToken)
        {
            var predicate = PredicateBuilder.New<Domain.Entities.Company>(true);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                predicate = predicate.And(company => company.Name.Contains(request.SearchTerm));
            }

            if (request.ServiceTypes.Count > 0)
            {
                predicate = predicate.And(company => 
                    company.ServiceTypes.Any(serviceType => request.ServiceTypes.Contains(serviceType.Id)));
            }

            if (request.ServiceSubTypes.Count > 0)
            {
                predicate = predicate.And(company =>
                    company.ServiceSubTypes.Any(serviceSubType => request.ServiceSubTypes.Contains(serviceSubType.Id)));
            }

            if (request.Regions.Count > 0)
            {
                predicate = predicate.And(company =>
                    company.Locations.Any(location => request.Regions.Contains(location.RegionId)));
            }

            if (request.Countries.Count > 0)
            {
                predicate = predicate.And(company =>
                    company.Locations.Any(location => request.Countries.Contains(location.CountryId)));
            }

            if (request.Industries.Count > 0)
            {
                predicate = predicate.And(company =>
                    company.Industries.Any(industry => request.Industries.Contains(industry.Id)));
            }

            if (request.Capabilities.Count > 0)
            {
                predicate = predicate.And(company =>
                    company.Capabilities.Any(capability => request.Capabilities.Contains(capability.Id)));
            }

            var companies = await _companyRepository.FindAllProjectToAsync<PartnerProfileListItem>(
                    predicate,
                    request.PageNo,
                    request.PageSize,
                    queryOptions => string.IsNullOrEmpty(request.OrderBy) ? queryOptions.OrderByDescending(c => c.CreatedDate) : queryOptions.OrderBy(request.OrderBy),
                    cancellationToken);
            return companies.MapToPagedResult();
        }
    }
}