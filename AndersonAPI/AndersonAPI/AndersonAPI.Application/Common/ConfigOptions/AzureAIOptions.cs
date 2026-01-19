namespace AndersonAPI.Application.Common.ConfigOptions
{
    public sealed class AzureAIOptions
    {
        public string Provider { get; init; } = "AzureOpenAI";
        public string Endpoint { get; init; } = string.Empty;
        public string ApiKey { get; init; } = string.Empty;
        public string DeploymentName { get; init; } = string.Empty;
        public string EmbeddingDeploymentName { get; init; } = string.Empty;
        public int EmbeddingDimension { get; init; } = 0;
        public bool UseEntraId { get; init; } = false;
    }
}
