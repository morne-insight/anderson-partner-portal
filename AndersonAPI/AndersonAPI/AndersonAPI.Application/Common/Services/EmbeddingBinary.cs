using System.Runtime.InteropServices;

namespace AndersonAPI.Application.Common.Services
{
    public static class EmbeddingBinary
    {
        //public static byte[] ToVarbinary(float[] vector)
        //{
        //    if (vector is null) throw new ArgumentNullException(nameof(vector));
        //    var bytes = new byte[vector.Length * sizeof(float)];
        //    MemoryMarshal.Cast<byte, float>(bytes).Set(vector);
        //    return bytes;
        //}

        //public static float[] FromVarbinary(byte[] bytes)
        //{
        //    if (bytes is null) throw new ArgumentNullException(nameof(bytes));
        //    if (bytes.Length % sizeof(float) != 0)
        //        throw new ArgumentException("Invalid embedding binary length.", nameof(bytes));

        //    var floats = MemoryMarshal.Cast<byte, float>(bytes).ToArray();
        //    return floats;
        //}

        public static byte[] ToVarbinary(float[] vector)
        {
            if (vector is null) throw new ArgumentNullException(nameof(vector));

            var bytes = new byte[vector.Length * sizeof(float)];
            Buffer.BlockCopy(vector, 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static float[] FromVarbinary(byte[] bytes)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length % sizeof(float) != 0)
                throw new ArgumentException("Invalid embedding binary length.", nameof(bytes));

            var vector = new float[bytes.Length / sizeof(float)];
            Buffer.BlockCopy(bytes, 0, vector, 0, bytes.Length);
            return vector;
        }
    }
}
