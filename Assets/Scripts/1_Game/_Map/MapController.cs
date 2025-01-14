using _0_Core;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game._Map {
    /// <summary>
    /// Simple MapController, doesnt actually control any of the map stuff
    /// Just a monobehaviour to visualize the size of the map
    /// </summary>
    public class MapController : MonoBehaviour {
        private readonly Vector2Int[] _cornerPositions = {
            new Vector2Int(-Map.Size / 2, -Map.Size / 2),
            new Vector2Int(Map.Size / 2, Map.Size / 2),
            new Vector2Int(-Map.Size / 2, Map.Size / 2),
            new Vector2Int(Map.Size / 2, -Map.Size / 2)
        };

        private void OnEnable() {
            MapGenerator.Generate();
            DrawMapMesh();
            SpawnMapCorners();
        }

        private void DrawMapMesh() {
            Vector3[] vertices = new Vector3[MapGenerator.Vertices.Length];
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i] = new Vector3(MapGenerator.Vertices[i].x, MapGenerator.Vertices[i].y, MapGenerator.Vertices[i].z);
            }
            
            Mesh mesh = new Mesh {
                vertices = vertices,
                triangles = MapGenerator.Triangles,
                uv = CalculateUVs(vertices),
                normals = CalculateFlatNormals(vertices, MapGenerator.Triangles)
            };
            mesh.RecalculateNormals();

            GameObject meshObject = new GameObject("mesh") {
                transform = {
                    position = new Vector3(0.5f, 0, 0.5f)
                },
                layer = (int)Mathf.Log(Globals.TERRAIN_LAYER_MASK, 2) // bitshift sorry!
            };
            meshObject.transform.SetParent(GameObject.Find("map").transform, false);

            MeshCollider meshCollider = meshObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            MeshFilter filter = meshObject.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshRenderer.material = Resources.Load("Materials/ground") as Material;
            filter.mesh = mesh;
        }
        
        private Vector3[] CalculateFlatNormals(Vector3[] vertices, int[] triangles) {
            Vector3[] normals = new Vector3[vertices.Length];
        
            for (int i = 0; i < triangles.Length; i += 3) {
                int index0 = triangles[i];
                int index1 = triangles[i + 1];
                int index2 = triangles[i + 2];
        
                Vector3 v0 = vertices[index0];
                Vector3 v1 = vertices[index1];
                Vector3 v2 = vertices[index2];
        
                Vector3 edge1 = v1 - v0;
                Vector3 edge2 = v2 - v0;
                Vector3 normal = Vector3.Cross(edge1, edge2).normalized;
        
                normals[index0] = normal;
                normals[index1] = normal;
                normals[index2] = normal;
            }
        
            return normals;
        }
        
        private Vector2[] CalculateUVs(Vector3[] vertices) {
            Vector2[] uvs = new Vector2[vertices.Length];

            for (int i = 0; i < vertices.Length; i++) {
                uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
            }

            return uvs;
        }

        private void SpawnMapCorners() {
            GameObject parent = new GameObject {
                name = "Map Corners"
            };

            for (int i = 0; i < 4; i++) {
                GameObject mapCorner = Instantiate(Resources.Load("Prefabs/Entity/Building/other/construction_pole"), parent.transform, false) as GameObject;
                if (mapCorner == null) {
                    Logger.Print("Cant find construction_pole in Resources", LogLevel.WARNING);
                    return;
                }

                mapCorner.transform.position = new Vector3(_cornerPositions[i].x, 0, _cornerPositions[i].y);
            }
        }
    }
}