using System;

namespace _0_Core.Noise {
    public static class Perlin {
        private static int[] permutationTable;
        private static int[] p;

        static Perlin() {
            permutationTable = new int[512];
            p = new int[256];

            for (int i = 0; i < 256; i++) {
                p[i] = i;
            }

            Random rand = new Random();
            for (int i = 0; i < 256; i++) {
                int j = rand.Next(256);
                (p[i], p[j]) = (p[j], p[i]);
            }

            for (int i = 0; i < 256; i++) {
                permutationTable[i] = p[i];
                permutationTable[i + 256] = p[i];
            }
        }

        public static float Noise(float x, float y) {
            int tempX = (int)Math.Floor(x) & 255;
            int tempY = (int)Math.Floor(y) & 255;

            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);

            float u = Fade(x);
            float v = Fade(y);

            int A = permutationTable[tempX] + tempY;
            int AA = permutationTable[A];
            int AB = permutationTable[A + 1];
            int B = permutationTable[tempX + 1] + tempY;
            int BA = permutationTable[B];
            int BB = permutationTable[B + 1];

            float gradAA = Grad(permutationTable[AA], x, y);
            float gradAB = Grad(permutationTable[AB], x, y - 1);
            float gradBA = Grad(permutationTable[BA], x - 1, y);
            float gradBB = Grad(permutationTable[BB], x - 1, y - 1);

            float lerpX1 = Lerp(gradAA, gradBA, u);
            float lerpX2 = Lerp(gradAB, gradBB, u);

            return Lerp(lerpX1, lerpX2, v);
        }

        private static float Fade(float t) {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float Lerp(float a, float b, float t) {
            return a + t * (b - a);
        }

        private static float Grad(int hash, float x, float y) {
            int h = hash & 15;
            float u = h < 8 ? x : y;
            float v = h < 4 ? y : (h == 12 || h == 14 ? x : 0);
            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }
    }
}