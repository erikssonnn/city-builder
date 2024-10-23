using _0_Core;
using _0_Core.Building;
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