using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Common.Services;
using AndersonAPI.Domain.Common.Exceptions;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Repositories;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandHandler", Version = "2.0")]

namespace AndersonAPI.Application.Companies.UpdateCompany
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmbeddingService _embeddingService;

        [IntentManaged(Mode.Merge)]
        public UpdateCompanyCommandHandler(ICompanyRepository companyRepository, IEmbeddingService embeddingService)
        {
            _companyRepository = companyRepository;
            _embeddingService = embeddingService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _companyRepository.FindByIdAsync(request.Id, cancellationToken);
            if (company is null)
            {
                throw new NotFoundException($"Could not find Company '{request.Id}'");
            }

            company.Update(
                request.Name,
                request.ShortDescription,
                request.Description,
                request.WebsiteUrl,
                request.EmployeeCount);

            var textToEmbed = $"Company: {company.Name},{Environment.NewLine}" +
                $"Capabilites: {string.Join(",", company.Capabilities.Select(c => c.Name))},{Environment.NewLine}" +
                $"Industries: {string.Join(",", company.Industries.Select(c => c.Name))},{Environment.NewLine}" +
                $"Regions: {string.Join(",", company.Locations.Select(c => c.Region.Name))},{Environment.NewLine}" +
                $"Countries: {string.Join(",", company.Locations.Select(c => c.Country.Name))},{Environment.NewLine}" +
                $"Description: {request.Description}";

            var embedding = await _embeddingService.EmbedAsync(textToEmbed, cancellationToken);
            
            var vector = EmbeddingBinary.ToVarbinary(embedding.Vector);

            company.SetEmbedding(vector, embedding.EmbeddingDeploymentName, embedding.Dimensions, DateTime.Now);

        }
    }
}