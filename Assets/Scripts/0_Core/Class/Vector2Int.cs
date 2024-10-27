using System;

namespace _0_Core.Class {
    /// <summary>
    /// Custom Vector2Int class to remove unity implementation in low level
    /// </summary>
    public readonly struct Vector2Int : IEquatable<Vector2Int> {
        public int x { get; } // keeping lowercase for consistency
        public int y { get; } // keeping lowercase for consistency

        public Vector2Int(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Vector2Int other) {
            return x == other.x && y == other.y;
        }

        public override bool Equals(object obj) {
            return obj is Vector2Int other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 31 + x;
                hash = hash * 31 + y;
                return hash;
            }
        }

        public static bool operator ==(Vector2Int left, Vector2Int right) { return left.Equals(right); }
        public static bool operator !=(Vector2Int left, Vector2Int right) { return !(left == right); }
        public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.x + b.x, a.y + b.y);
        public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.x - b.x, a.y - b.y);
        public override string ToString() => $"({x}, {y})";
    }
}