using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Common.Pagination;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryModels", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetPartnersDirectory
{
    public class GetPartnersDirectoryQuery : IRequest<PagedResult<PartnerProfileListItem>>, IQuery
    {
        public GetPartnersDirectoryQuery(int pageNo,
            int pageSize,
            Guid? serviceType,
            string? searchTerm,
            string? orderBy,
            List<Guid> industries,
            List<Guid> capabilities,
            List<Guid> countries,
            List<Guid> regions)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            ServiceType = serviceType;
            SearchTerm = searchTerm;
            OrderBy = orderBy;
            Industries = industries;
            Capabilities = capabilities;
            Countries = countries;
            Regions = regions;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public Guid? ServiceType { get; set; }
        public List<Guid> Regions { get; set; }
        public List<Guid> Countries { get; set; }
        public List<Guid> Capabilities { get; set; }
        public List<Guid> Industries { get; set; }
    }
}