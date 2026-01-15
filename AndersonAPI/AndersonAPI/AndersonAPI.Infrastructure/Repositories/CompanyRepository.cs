using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using AndersonAPI.Infrastructure.Persistence;
using AutoMapper;
using Intent.RoslynWeaver.Attributes;
using Microsoft.EntityFrameworkCore;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.EntityFrameworkCore.Repositories.Repository", Version = "1.0")]

namespace AndersonAPI.Infrastructure.Repositories
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CompanyRepository : RepositoryBase<Company, Company, ApplicationDbContext>, ICompanyRepository
    {
        public CompanyRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<TProjection?> FindByIdProjectToAsync<TProjection>(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await FindProjectToAsync<TProjection>(x => x.Id == id, cancellationToken);
        }

        public async Task<Company?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Company?> FindByIdAsync(
            Guid id,
            Func<IQueryable<Company>, IQueryable<Company>> queryOptions,
            CancellationToken cancellationToken = default)
        {
            return await FindAsync(x => x.Id == id, queryOptions, cancellationToken);
        }

        public async Task<List<Company>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            // Force materialization - Some combinations of .net9 runtime and EF runtime crash with "Convert ReadOnlySpan to List since expression trees can't handle ref struct"
            var idList = ids.ToList();
            return await FindAllAsync(x => idList.Contains(x.Id), cancellationToken);
        }
        
        [IntentIgnore]
        public async Task<List<Capability>> GetCapabilitiesByIdsAsync(List<Guid> capabilityIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Capability>()           
                .Where(c => capabilityIds.Contains(c.Id))
                .ToListAsync(cancellationToken);
        }
        
        [IntentIgnore]
        public async Task<List<Industry>> GetIndustriesByIdsAsync(List<Guid> industryIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<Industry>()           
                .Where(i => industryIds.Contains(i.Id))
                .ToListAsync(cancellationToken);
        }
        
        [IntentIgnore]
        public async Task<List<ServiceType>> GetServiceTypesByIdsAsync(List<Guid> serviceTypeIds, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Set<ServiceType>()           
                .Where(s => serviceTypeIds.Contains(s.Id))
                .ToListAsync(cancellationToken);
        }
    }
}