using System.Text.Json.Serialization;

namespace AndersonAPI.Infrastructure.Services.AzureOenAi
{
    public sealed class EmbeddingRequest
    {
        public EmbeddingRequest(string input, string model, int? dimensions)
        {
            Input = input;
            Model = model;
            if(dimensions != null)
            {
                Dimensions = dimensions.Value;
            }
        }

        [JsonPropertyName("input")]
        public string Input { get; private set; }

        [JsonPropertyName("model")]
        public string Model { get; private set; }

        [JsonPropertyName("dimensions")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Dimensions { get; private set; }
    }

    public sealed class EmbeddingsResponse
    {
        [JsonPropertyName("data")]
        public List<EmbeddingData> Data { get; set; } = new();

        [JsonPropertyName("model")]
        public string Model { get; set; } = default!;
    }

    public sealed class EmbeddingData
    {
        [JsonPropertyName("embedding")]
        public float[] Embedding { get; set; } = Array.Empty<float>();
    }
}
