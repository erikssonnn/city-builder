using System.Collections.Generic;
using _0_Core;
using _0_Core.Class;
using _0_Core.Entity.Terrain;
using _0_Core.Map;
using _1_Game._HelpFunctions;
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
            MapGenerator.GenerateVertices();
            SpawnMapCorners();
            GenerateMesh();
        }

        private static void GenerateMesh() {
            Mesh mesh = new Mesh() {
                vertices = HelpFunctions.Vector3IntToVector3(MapGenerator.Vertices),
                triangles = MapGenerator.Triangles,
                uv = HelpFunctions.Vector2IntToVector2(MapGenerator.Uvs),
            };

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
            filter.mesh = mesh;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();

            float[,] terrainMap = GenerateTerrainMap();
            Texture2D texture = GenerateTexture(terrainMap);
            SetWaterOnMap(terrainMap);
            meshRenderer.material = Resources.Load("Materials/ground") as Material;
            if (meshRenderer.material == null) {
                Logger.Print("Failed to load Materials/ground from Resources!", LogLevel.ERROR);
                return;
            }

            meshRenderer.material.mainTexture = texture;
        }

        private static void SetWaterOnMap(float[,] terrainMap) {
            Tile tile = new Tile(TileType.TERRAIN);
            int offset = Map.Size / 2;
            List<Vector2Int> positions = new List<Vector2Int>();
            for (int x = 0; x < terrainMap.GetLength(0); x++) {
                for (int y = 0; y < terrainMap.GetLength(1); y++) {
                    if (terrainMap[x, y] < 0.25f) {
                        positions.Add(new Vector2Int(x - offset, y - offset));
                    }
                }
            }
            if (positions.Count > 0) {
                Map.SetPositions(positions.ToArray(), tile);
            }
        }

        private static Color32 RandomBetweenColors(Color32 col1, Color32 col2) {
            return MathE.RandomValue() > 0.5f ? col1 : col2;
        }

        private static Color32 CalculateColorFromTerrainMap(float value) {
            if (value < 0.25f) return Color.blue;
            if (value < 0.5f) return RandomBetweenColors(new Color32(126, 107, 2, 255), new Color32(107, 90, 62, 255));
            if (value < 0.75f) return RandomBetweenColors(new Color32(107, 106, 52, 255), new Color32(130, 128, 63, 255));
            return RandomBetweenColors(new Color32(107, 106, 52, 255), new Color32(130, 128, 63, 255));
        }

        private static Color32 ValueToColor(float value) => new Color32((byte)(value * 255), (byte)(value * 255), (byte)(value * 255), 255);

        private static float[,] GenerateTerrainMap() {
            float[,] terrainMap = new float[Map.Size, Map.Size];
            for (int x = 0; x < Map.Size; x++) {
                for (int y = 0; y < Map.Size; y++) {
                    terrainMap[x, y] = Mathf.PerlinNoise(x * 0.05f, y * 0.05f);
                }
            }

            return terrainMap;
        }

        private static Texture2D GenerateTexture(float[,] terrainMap) {
            Texture2D texture = new Texture2D(Map.Size, Map.Size, TextureFormat.RGBA32, false);

            for (int y = 0; y < Map.Size; y++) {
                for (int x = 0; x < Map.Size; x++) {
                    float value = terrainMap[x, y];
                    texture.SetPixel(x, y, CalculateColorFromTerrainMap(value));
                }
            }

            texture.filterMode = FilterMode.Point;
            texture.Apply();
            return texture;
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