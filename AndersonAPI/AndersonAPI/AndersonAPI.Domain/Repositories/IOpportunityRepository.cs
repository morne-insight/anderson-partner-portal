using AndersonAPI.Domain.Entities;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.Repositories.Api.EntityRepositoryInterface", Version = "1.0")]

namespace AndersonAPI.Domain.Repositories
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public interface IOpportunityRepository : IEFRepository<Opportunity, Opportunity>
    {
        [IntentManaged(Mode.Fully)]
        Task<TProjection?> FindByIdProjectToAsync<TProjection>(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<Opportunity?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<Opportunity?> FindByIdAsync(Guid id, Func<IQueryable<Opportunity>, IQueryable<Opportunity>> queryOptions, CancellationToken cancellationToken = default);
        [IntentManaged(Mode.Fully)]
        Task<List<Opportunity>> FindByIdsAsync(Guid[] ids, CancellationToken cancellationToken = default);
        //[IntentIgnore]
        //Task<List<TProjection>> FindOpportunitiesByCompanyIdsProjectToAsync<TProjection>(List<Guid> companyIds, CancellationToken cancellationToken = default);
    }
}