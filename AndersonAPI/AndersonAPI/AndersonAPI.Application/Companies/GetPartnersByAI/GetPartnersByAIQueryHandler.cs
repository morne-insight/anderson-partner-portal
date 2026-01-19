using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Common.Services;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.QueryHandler", Version = "1.0")]

namespace AndersonAPI.Application.Companies.GetPartnersByAI
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class GetPartnersByAIQueryHandler : IRequestHandler<GetPartnersByAIQuery, List<PartnerProfileListItem>>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmbeddingService _embeddingService;

        [IntentManaged(Mode.Merge)]
        public GetPartnersByAIQueryHandler(ICompanyRepository companyRepository, IEmbeddingService embeddingService)
        {
            _companyRepository = companyRepository;
            _embeddingService = embeddingService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public async Task<List<PartnerProfileListItem>> Handle(
            GetPartnersByAIQuery request,
            CancellationToken cancellationToken)
        {
            var textToEmbed = $"{request.Query}";
            var embedding = await _embeddingService.EmbedAsync(textToEmbed, cancellationToken);

            var searchCompanies = await _companyRepository
                .FindAllProjectToAsync<CompanySearchDto>(
                    c => c.State == Domain.EntityState.Enabled &&
                    c.Embedding != null,
                    cancellationToken);

            var foundCompanies = new List<CompanySearchDto>(searchCompanies.Count);

            foreach (var company in searchCompanies)
            {
                if (company.Embedding == null || company.Embedding.Length == 0)
                {
                    continue;
                }
                var companyEmbedding = EmbeddingBinary.FromVarbinary(company.Embedding);
                var similarity = Cosine.Similarity(embedding.Vector, companyEmbedding);
                foundCompanies.Add(CompanySearchDto.Create(
                    company.Id,
                    company.Name,
                    company.Embedding,
                    similarity));
            }

            var hits = foundCompanies
                .OrderByDescending(h => h.Score)
                .Take(3)
                .Select(h => new { h.Id, h.Score })
                .ToList();

            // lookup for scores
            var scoreById = hits.ToDictionary(x => x.Id, x => x.Score);

            // ids for DB query
            var ids = hits.Select(x => x.Id).ToList();

            var companies = await _companyRepository
                .FindAllProjectToAsync<PartnerProfileListItem>(
                    c => ids.Contains(c.Id),
                    cancellationToken);

            // attach scores + keep the same order as hits
            foreach (var c in companies)
            {
                if (scoreById.TryGetValue(c.Id, out var score))
                    c.MatchScore = score * 100;
            }

            companies = companies
                .OrderByDescending(c => c.MatchScore)
                .ToList();

            return companies;
        }

    }
}