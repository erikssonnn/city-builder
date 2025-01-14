using System;

namespace _0_Core.Class {
    /// <summary>
    /// Custom Math class to remove unity implementation in low level
    /// </summary>
    public struct MathE {
        private static readonly Random _random = new Random();

        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        public static float RandomValue() => (float)_random.NextDouble();
    }
}