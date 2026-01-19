namespace AndersonAPI.Application.Common.Models
{
    public sealed record EmbeddingResult(float[] Vector, string EmbeddingDeploymentName, int Dimensions);
}
