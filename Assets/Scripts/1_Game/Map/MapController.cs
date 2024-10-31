using System;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;
using Vector3Int = _0_Core.Class.Vector3Int;

namespace _1_Game {
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
                triangles = MapGenerator.Triangles
            };
            mesh.RecalculateNormals();

            GameObject meshObject = new GameObject {
                name = "mapMesh"
            };
            meshObject.transform.SetParent(transform, false);
            MeshFilter filter = meshObject.AddComponent<MeshFilter>();
            meshObject.AddComponent<MeshRenderer>();
            filter.mesh = mesh; }

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