using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.Repositories.Api.EntityRepositoryInterface", Version = "1.0")]

namespace AndersonAPI.Domain.Repositories
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public interface ICompanyRepository : IEFRepository<Company, Company>
    {
        [IntentManaged(Mode.Fully)]
        Task<TProjection?> FindByIdProjectToAsync<TProjection>(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<Company?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<Company?> FindByIdAsync(Guid id, Func<IQueryable<Company>, IQueryable<Company>> queryOptions, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<List<Company>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
        
        [IntentIgnore]
        Task<List<Capability>> GetCapabilitiesByIdsAsync(List<Guid> capabilityIds, CancellationToken cancellationToken = default);
        
        [IntentIgnore]
        Task<List<Industry>> GetIndustriesByIdsAsync(List<Guid> industryIds, CancellationToken cancellationToken = default);
        
        [IntentIgnore]
        Task<List<ServiceType>> GetServiceTypesByIdsAsync(List<Guid> serviceTypeIds, CancellationToken cancellationToken = default);
    }
}