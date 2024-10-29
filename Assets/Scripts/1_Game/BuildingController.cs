using _0_Core;
using _0_Core.Building;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game {
    /// <summary>
    /// Controls when and how to create and place buildings in the world
    /// </summary>
    public class BuildingController : MonoBehaviour {
        private static BuildingController _instance;
        private Building _currentBuilding;
        private Camera _cam;

        private void OnDrawGizmos() {
            if (_currentBuilding == null || _currentBuilding.Grid == null || _currentBuilding.Grid.Length == 0) {
                return;
            }

            foreach (Vector2Int pos in _currentBuilding.Grid) {
                Gizmos.color = Color.magenta;
                Gizmos.DrawCube(new Vector3(pos.x, 0, pos.y), new Vector3(1f, 0.1f, 1f));
            }
        }

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

            _cam = Camera.main;
        }

        private void OnDestroy() => Map.Clear();

        private void Update() {
            if (_currentBuilding == null) {
                return;
            }

            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
                Vector2Int pos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
                _currentBuilding.SetPosition(pos);
                _currentBuilding.SetGrid(GetGridPositions(pos));
                _currentBuilding.CheckValidPlacement();
                _currentBuilding.SetMaterial();
            }
        }

        public void RotateBuilding() {
            if (_currentBuilding == null || _currentBuilding.IsFixed) {
                return;
            }
            _currentBuilding.Rotate();
        }

        public void PlaceBuilding() {
            if (_currentBuilding == null || _currentBuilding.HasValidPlacement == false) {
                return;
            }

            Map.SetPositions(_currentBuilding.Grid, _currentBuilding);
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

        private Vector2Int[] GetGridPositions(Vector2Int pos) {
            int totalPositions = (_currentBuilding.Size * 2 + 1) * (_currentBuilding.Size * 2 + 1);
            Vector2Int[] grid = new Vector2Int[totalPositions];

            int index = 0;
            for (int x = -_currentBuilding.Size; x <= _currentBuilding.Size; x++) {
                for (int y = -_currentBuilding.Size; y <= _currentBuilding.Size; y++) {
                    Vector2Int gridPos = new Vector2Int(pos.x + x, pos.y + y);
                    grid[index++] = gridPos;
                }
            }

            return grid;
        }
    }
}