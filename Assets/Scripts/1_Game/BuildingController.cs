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
        private Material _validMaterial;
        private Material _invalidMaterial;

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

            _validMaterial = Resources.Load( $"Materials/valid") as Material;
            _invalidMaterial = Resources.Load( $"Materials/invalid") as Material;
        }

        private void Update() {
            if (_currentBuilding == null) { return; }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
                Vector3Int pos = new Vector3Int(Mathf.FloorToInt(hit.point.x), 0, Mathf.FloorToInt(hit.point.z));
                _currentBuilding.SetPosition(pos);
                _currentBuilding.SetMaterial(Map.IsFree(new Vector2Int(pos.x, pos.z)) 
                    ? _validMaterial
                    : _invalidMaterial);
            }
        }

        public void StartPlacingBuilding(int buildingIndex) {
            if (_currentBuilding != null) {
                Destroy(_currentBuilding.Transform.gameObject);
            }

            Building building = new Building(Globals.BUILDING_DATA[buildingIndex]);
            Logger.Print($"START PLACING BUILDING {building.Name}");
            _currentBuilding = building;
        }

        public void CancelPlacingBuilding() {
            if (_currentBuilding == null) { return; }
            Destroy(_currentBuilding.Transform.gameObject);
            _currentBuilding = null;
        }
    }
}