using AndersonAPI.Application.Common.ConfigOptions;
using AndersonAPI.Application.Common.Interfaces;
using AndersonAPI.Application.Common.Models;
using AndersonAPI.Infrastructure.Services.AzureOenAi;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace AndersonAPI.Infrastructure.Services
{
    public sealed class EmbeddingService : IEmbeddingService
    {
        private readonly HttpClient _http;
        private readonly AzureAIOptions _options;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

        public EmbeddingService(HttpClient http, IOptions<AzureAIOptions> options)
        {
            _http = http;
            _options = options.Value;
        }

        public async Task<EmbeddingResult> EmbedAsync(string input, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Input must not be empty", nameof(input));

            var req = new EmbeddingRequest(input, _options.EmbeddingDeploymentName, _options.EmbeddingDimension);

            using var msg = new HttpRequestMessage(HttpMethod.Post, "embeddings");
            msg.Headers.Add("api-key", _options.ApiKey);
            msg.Content = new StringContent(JsonSerializer.Serialize(req, JsonOptions), Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(msg, cancellationToken);
            var body = await resp.Content.ReadAsStringAsync(cancellationToken);

            if (!resp.IsSuccessStatusCode)
                throw new HttpRequestException($"Embeddings call failed: {(int)resp.StatusCode} {resp.ReasonPhrase}\n{body}");

            var parsed = JsonSerializer.Deserialize<EmbeddingsResponse>(body, JsonOptions)
                         ?? throw new InvalidOperationException("Failed to parse embeddings response.");

            var vector = parsed.Data.FirstOrDefault()?.Embedding
                         ?? throw new InvalidOperationException("Embeddings response contained no vector.");

            return new EmbeddingResult(vector, parsed.Model, vector.Length);
        }
    }
}
