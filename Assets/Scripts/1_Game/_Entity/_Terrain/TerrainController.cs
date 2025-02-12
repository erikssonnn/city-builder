using _0_Core;
using _0_Core.Entity.Terrain;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Terrain = _0_Core.Entity.Terrain;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game._Entity._Terrain {
    /// <summary>
    /// Only used to create terrain objects and destroying them using unity
    /// For the generation code see TerrainGenerator.cs
    /// </summary>
    public class TerrainController : MonoBehaviour {
        private UnityEngine.Camera _cam;
        private static TerrainController _instance;

        public static TerrainController Instance {
            get {
                if (_instance == null) {
                    Logger.Print("TerrainController is NULL", LogLevel.FATAL);
                }

                return _instance;
            }
        }

        private void OnEnable() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            
            _cam = UnityEngine.Camera.main;
            TerrainGenerator.Generate();
        }

        public void DeleteTerrain() {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
                return;
            }
            Vector2Int pos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
            Terrain.Terrain terrain = Map.GetTerrainFromPosition(pos);

            if (terrain == null) {
                return;
            }
            Map.DeleteTerrainPositions(terrain);
            terrain.Destroy();
        }
    }
}