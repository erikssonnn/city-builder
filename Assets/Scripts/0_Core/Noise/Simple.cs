namespace _0_Core.Noise {
    public class Simple {
        public static float Noise(float x, float y)
        {
            int n = (int)(x + y * 57);
            n = (n << 13) ^ n;
            int result = (n * (n * n * 60493 + 19990303) + 1376312589) & 0x7fffffff;
            
            return (1.0f - (result / 1073741824.0f));
        }
    }
}