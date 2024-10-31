using _0_Core.Class;

namespace _0_Core.Map {
    /// <summary>
    /// Used to generate the map mesh
    /// This passes the map information to a MapController which draws the mesh
    /// </summary>
    public static class MapGenerator {
        public static Vector3Int[] Vertices { get; private set; }
        public static int[] Triangles { get; private set; }

        public static void Generate() {
            Vertices = new Vector3Int[] {
                new Vector3Int(0, 0, 0), // Bottom back left
                new Vector3Int(1, 0, 0), // Bottom back right
                new Vector3Int(1, 1, 0), // Top back right
                new Vector3Int(0, 1, 0), // Top back left
                new Vector3Int(0, 0, 1), // Bottom front left
                new Vector3Int(1, 0, 1), // Bottom front right
                new Vector3Int(1, 1, 1), // Top front right
                new Vector3Int(0, 1, 1) // Top front left
            };

            Triangles = new int[] {
                0, 2, 1, 0, 3, 2, // Back face
                4, 5, 6, 4, 6, 7, // Front face
                0, 1, 5, 0, 5, 4, // Bottom face
                2, 3, 7, 2, 7, 6, // Top face
                0, 4, 7, 0, 7, 3, // Left face
                1, 2, 6, 1, 6, 5 // Right face
            };
        }
    }
}