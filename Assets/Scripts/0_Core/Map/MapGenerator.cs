using System.Collections.Generic;
using System.Linq;
using _0_Core.Class;
using _0_Core.Noise;
using erikssonn;
using UnityEngine;
using Vector3Int = _0_Core.Class.Vector3Int;

namespace _0_Core.Map {
    /// <summary>
    /// Used to generate the map mesh
    /// This passes the map information to a MapController which draws the mesh
    /// </summary>
    public static class MapGenerator {
        public static Vector3Int[] Vertices { get; private set; }
        public static int[] Triangles { get; private set; }

        public static void Generate() {
            Dictionary<Vector3Int, int> vertexLookup = new Dictionary<Vector3Int, int>();
            List<Vector3Int> vertices = new List<Vector3Int>();
            List<int> triangles = new List<int>();

            Vector3Int[] cubeVertices = {
                new Vector3Int(0, 0, 0), new Vector3Int(1, 0, 0), new Vector3Int(1, 1, 0), new Vector3Int(0, 1, 0),
                new Vector3Int(0, 0, 1), new Vector3Int(1, 0, 1), new Vector3Int(1, 1, 1), new Vector3Int(0, 1, 1)
            };

            int[] cubeTriangles = {
                0, 2, 1, 0, 3, 2,
                4, 5, 6, 4, 6, 7,
                0, 1, 5, 0, 5, 4,
                2, 3, 7, 2, 7, 6,
                0, 4, 7, 0, 7, 3,
                1, 2, 6, 1, 6, 5
            };
            
            for (int x = -Map.Size / 2 - 1; x < Map.Size / 2; x++) { // -1 or it skips the first row
                for (int y = -Map.Size / 2 - 1; y < Map.Size / 2; y++) {
                    Vector3Int cubeOffset = new Vector3Int(x, -1, y); // -1 or its to high up

                    // when adding vertex, check if it exists and use it as a shared vertex
                    foreach (Vector3Int vert in cubeVertices) {
                        Vector3Int globalVertexPos = vert + cubeOffset;

                        if (vertexLookup.TryGetValue(globalVertexPos, out int vertexIndex)) {
                            continue;
                        }
                        vertexIndex = vertices.Count;
                        vertexLookup[globalVertexPos] = vertexIndex;
                        vertices.Add(globalVertexPos);
                    }

                    triangles.AddRange(cubeTriangles.Select(triangle => vertexLookup[cubeVertices[triangle] + cubeOffset]));
                }
            }

            Vertices = vertices.ToArray();
            Triangles = triangles.ToArray();
        }
    }
}