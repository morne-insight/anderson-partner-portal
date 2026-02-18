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
            List<Guid> serviceTypes,
            List<Guid> serviceSubTypes,
            List<Guid> industries,
            List<Guid> capabilities,
            List<Guid> countries,
            List<Guid> regions,
            string? searchTerm,
            string? orderBy)
        {
            PageNo = pageNo;
            PageSize = pageSize;
            ServiceTypes = serviceTypes;
            ServiceSubTypes = serviceSubTypes;
            Industries = industries;
            Capabilities = capabilities;
            Countries = countries;
            Regions = regions;
            SearchTerm = searchTerm;
            OrderBy = orderBy;
        }

        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public List<Guid> ServiceTypes { get; set; }
        public List<Guid> ServiceSubTypes { get; set; }
        public string? OrderBy { get; set; }
        public string? SearchTerm { get; set; }
        public List<Guid> Regions { get; set; }
        public List<Guid> Countries { get; set; }
        public List<Guid> Capabilities { get; set; }
        public List<Guid> Industries { get; set; }
        public List<Guid> CoreServices { get; set; }
    }
}