using Vector2Int = _0_Core.Class.Vector2Int;
using Vector3Int = _0_Core.Class.Vector3Int;

namespace _0_Core.Map {
    /// <summary>
    /// Used to generate the map mesh
    /// This passes the map information to a MapController which draws the mesh
    /// </summary>
    public static class MapGenerator {
        public static Vector3Int[] Vertices { get; private set; }
        public static int[] Triangles { get; private set; }
        public static Vector2Int[] Uvs { get; private set; }

        public static void GenerateVertices() {
            Vertices = new[] {
                new Vector3Int(-Map.Size / 2, 0, -Map.Size / 2),
                new Vector3Int(Map.Size / 2, 0, -Map.Size / 2),
                new Vector3Int(-Map.Size / 2, 0, Map.Size / 2),
                new Vector3Int(Map.Size / 2, 0, Map.Size / 2)
            };

            Triangles = new[] {
                2, 1, 0,
                2, 3, 1
            };

            Uvs = new[] {
                new Vector2Int(0, 0),
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(1, 1)
            };
        }
    }
}