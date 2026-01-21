using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Common.Models;
using AndersonAPI.Application.Common.Services;
using AndersonAPI.Domain.Entities;
using AndersonAPI.Domain.Events;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.MediatR.DomainEvents.DefaultDomainEventHandler", Version = "1.0")]

namespace AndersonAPI.Application.EventHandlers
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class CompanyUpdatedEventHandler : INotificationHandler<DomainEventNotification<CompanyUpdatedEvent>>
    {
        private readonly IEmbeddingService _embeddingService;

        [IntentManaged(Mode.Merge)]
        public CompanyUpdatedEventHandler(IEmbeddingService embeddingService)
        {
            _embeddingService = embeddingService;
        }

        [IntentManaged(Mode.Fully, Body = Mode.Merge)]
        public async Task Handle(
            DomainEventNotification<CompanyUpdatedEvent> notification,
            CancellationToken cancellationToken)
        {
            var company = notification.DomainEvent.Company;

            var textToEmbed = $"Company: {company.Name},{Environment.NewLine}" +
                $"Capabilites: {string.Join(",", company.Capabilities.Select(c => c.Name))},{Environment.NewLine}" +
                $"Industries: {string.Join(",", company.Industries.Select(c => c.Name))},{Environment.NewLine}" +
                $"Regions: {string.Join(",", company.Locations.Select(c => c.Region.Name))},{Environment.NewLine}" +
                $"Countries: {string.Join(",", company.Locations.Select(c => c.Country.Name))},{Environment.NewLine}" +
                $"Description: {company.FullDescription}";

            var embedding = await _embeddingService.EmbedAsync(textToEmbed, cancellationToken);

            var vector = EmbeddingBinary.ToVarbinary(embedding.Vector);

            company.SetEmbedding(vector, embedding.EmbeddingDeploymentName, embedding.Dimensions, DateTime.Now);
        }
    }
}