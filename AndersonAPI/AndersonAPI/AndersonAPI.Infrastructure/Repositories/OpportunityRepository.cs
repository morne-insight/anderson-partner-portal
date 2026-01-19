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
    public class OpportunityRepository : RepositoryBase<Opportunity, Opportunity, ApplicationDbContext>, IOpportunityRepository
    {
        public OpportunityRepository(ApplicationDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<TProjection?> FindByIdProjectToAsync<TProjection>(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await FindProjectToAsync<TProjection>(x => x.Id == id, cancellationToken);
        }

        public async Task<Opportunity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await FindAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Opportunity?> FindByIdAsync(
            Guid id,
            Func<IQueryable<Opportunity>, IQueryable<Opportunity>> queryOptions,
            CancellationToken cancellationToken = default)
        {
            return await FindAsync(x => x.Id == id, queryOptions, cancellationToken);
        }

        public async Task<List<Opportunity>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default)
        {
            // Force materialization - Some combinations of .net9 runtime and EF runtime crash with "Convert ReadOnlySpan to List since expression trees can't handle ref struct"
            var idList = ids.ToList();
            return await FindAllAsync(x => idList.Contains(x.Id), cancellationToken);
        }
        
        //[IntentIgnore]
        //public async Task<List<TProjection>> FindOpportunitiesByCompanyIdsProjectToAsync<TProjection>(List<Guid> companyIds, CancellationToken cancellationToken = default)
        //{
        //    var query = _dbContext.Set<Opportunity>().AsQueryable();
        //    return await query
        //        .Where(o => companyIds.Contains(o.CompanyId))
        //        .Include(o => o.Country)
        //        .Include(o => o.OpportunityType)
        //        .Include(o => o.InterestedPartners)
        //        .ProjectTo<TProjection>(x => x, cancellationToken)
        //        .ToListAsync(cancellationToken);
        //}
    }
}