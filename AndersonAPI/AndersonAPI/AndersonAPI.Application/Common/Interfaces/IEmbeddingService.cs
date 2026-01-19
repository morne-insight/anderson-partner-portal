using AndersonAPI.Application.Common.Models;

namespace AndersonAPI.Application.Common.Interfaces
{
    public interface IEmbeddingService
    {
        Task<EmbeddingResult> EmbedAsync(string input, CancellationToken cancellationToken = default);
    }
}
