using System;
using Vector2 = System.Numerics.Vector2;

namespace _0_Core.Noise {
    public static class Perlin {
        private static float Interpolate(float a, float b, float w) {
            if (0.0f > w)
                return a;
            if (1.0f < w)
                return b;

            return (b - a) * w + a;
        }

        private static Vector2 RandomGradient(int ix, int iy) {
            const int w = 8 * sizeof(int);
            const int s = w / 2;

            uint a = (uint)ix;
            uint b = (uint)iy;

            a *= 3284157443;
            b ^= a << s | a >> w - s;
            b *= 1911520717;
            a ^= b << s | b >> w - s;
            a *= 2048419325;

            float random = a * ((float)3.14159265 / ~(~0u >> 1));
            return new Vector2((float)Math.Cos(random), (float)Math.Sin(random));
        }

        private static float DotGridGradient(int ix, int iy, float x, float y) {
            Vector2 gradient = RandomGradient(ix, iy);

            float dx = x - ix;
            float dy = y - iy;

            return (dx * gradient.X + dy * gradient.Y);
        }

        public static float Noise(float x, float y) {
            int x0 = (int)Math.Floor(x);
            int x1 = x0 + 1;
            int y0 = (int)Math.Floor(y);
            int y1 = y0 + 1;

            float sx = x - x0;
            float sy = y - y0;

            float n0 = DotGridGradient(x0, y0, x, y);
            float n1 = DotGridGradient(x1, y0, x, y);
            float ix0 = Interpolate(n0, n1, sx);

            n0 = DotGridGradient(x0, y1, x, y);
            n1 = DotGridGradient(x1, y1, x, y);
            float ix1 = Interpolate(n0, n1, sx);

            return Interpolate(ix0, ix1, sy);
        }
    }
}