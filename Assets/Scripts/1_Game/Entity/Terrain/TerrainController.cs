using _0_Core;
using _0_Core.Entity.Terrain;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game.Entity.Terrain {
    /// <summary>
    /// Only used to create terrain objects and destroying them using unity
    /// For the generation code see TerrainGenerator.cs
    /// </summary>
    public class TerrainController : MonoBehaviour {
        private Camera _cam;
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
                Destroy(this.gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            
            _cam = Camera.main;
            TerrainGenerator.Generate();
        }

        public void DeleteTerrain() {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
                Vector2Int pos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
                _0_Core.Entity.Terrain.Terrain terrain = Map.GetTerrainFromPosition(pos);
                
                if (terrain != null) {
                    Map.DeleteTerrainPositions(terrain);
                    terrain.Destroy();
                }
            }
        }
    }
}