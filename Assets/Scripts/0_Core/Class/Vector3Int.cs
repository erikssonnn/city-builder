using System;

namespace _0_Core.Class {
    /// <summary>
    /// Custom Vector3Int class to remove unity implementation in low level
    /// </summary>
    public readonly struct Vector3Int : IEquatable<Vector3Int> {
        // keeping lowercase for consistency
        public int x { get; }
        public int y { get; }
        public int z { get; } 

        public Vector3Int(int x, int y, int z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public bool Equals(Vector3Int other) {
            return x == other.x
                   && y == other.y
                   && z == other.z;
        }

        public override bool Equals(object obj) {
            return obj is Vector3Int other && Equals(other);
        }

        public override int GetHashCode() {
            unchecked {
                int hash = 17;
                hash = hash * 31 + x;
                hash = hash * 31 + y;
                return hash;
            }
        }

        public static bool operator ==(Vector3Int left, Vector3Int right) { return left.Equals(right); }
        public static bool operator !=(Vector3Int left, Vector3Int right) { return !(left == right); }
        public static Vector3Int operator +(Vector3Int a, Vector3Int b) => new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Vector3Int operator -(Vector3Int a, Vector3Int b) => new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        public override string ToString() => $"({x}, {y}, {z})";
    }
}