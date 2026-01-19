namespace AndersonAPI.Application.Common.Services
{
    public static class Cosine
    {
        public static double Similarity(ReadOnlySpan<float> a, ReadOnlySpan<float> b)
        {
            if (a.Length != b.Length) throw new ArgumentException("Vector dimensions must match.");

            double dot = 0;
            double normA = 0;
            double normB = 0;

            for (int i = 0; i < a.Length; i++)
            {
                var x = a[i];
                var y = b[i];
                dot += x * y;
                normA += x * x;
                normB += y * y;
            }

            if (normA <= 0 || normB <= 0) return 0;
            return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }
    }
}
