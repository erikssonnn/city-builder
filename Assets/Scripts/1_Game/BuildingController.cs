using _0_Core;
using _0_Core.Building;
using _0_Core.Input;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace _1_Game {
    /// <summary>
    /// Controls when and how to create and place buildings in the world
    /// </summary>
    public class BuildingController : MonoBehaviour {
        private static BuildingController _instance;
        private Building _currentBuilding;

        public static BuildingController Instance {
            get {
                if (_instance == null) {
                    Logger.Print("BuildingController is NULL", LogLevel.FATAL);
                }

                return _instance;
            }
        }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Update() {
            if (_currentBuilding == null) { return; }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
                Vector3Int pos = new Vector3Int(Mathf.FloorToInt(hit.point.x), 0, Mathf.FloorToInt(hit.point.z));
                _currentBuilding.SetPosition(pos);
                _currentBuilding.CheckValidPlacement(pos);
                _currentBuilding.SetMaterial();
            }
        }

        public void PlaceBuilding() {
            if (_currentBuilding == null || _currentBuilding.HasValidPlacement == false) { return; }
            Map.SetTile(new Vector2Int(_currentBuilding.Position.x, _currentBuilding.Position.z), new Tile(TileType.BUILDING, _currentBuilding));
            _currentBuilding.Place();
            _currentBuilding = null;
        }

        public void StartPlacingBuilding(int buildingIndex) {
            if (_currentBuilding != null && !_currentBuilding.IsFixed) {
                Destroy(_currentBuilding.Transform.gameObject);
            }

            Building building = new Building(Globals.BUILDING_DATA[buildingIndex]);
            _currentBuilding = building;
        }

        public void CancelPlacingBuilding() {
            if (_currentBuilding == null) {
                return;
            }

            Destroy(_currentBuilding.Transform.gameObject);
            _currentBuilding = null;
        }
    }
}