using _0_Core.Input;
using UnityEngine;
using erikssonn;
using Logger = erikssonn.Logger;

namespace _1_Game {
    /// <summary>
    /// Checks for player input every tick. Check the static InputMapper keycode defines
    /// </summary>
    public class InputController : MonoBehaviour {
        
        private static InputController _instance;

        public static InputController Instance {
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
            // BUILDING 1
            if (Input.GetKeyDown(InputMapper.BUILDING_1.KeyCode)) {
                BuildingController.Instance.StartPlacingBuilding(0);
            }
            // CANCEL BUILDING
            if (Input.GetKeyDown(InputMapper.CANCEL_BUILDING.KeyCode) || Input.GetMouseButtonDown(InputMapper.CANCEL_BUILDING.MouseButton)) {
                BuildingController.Instance.CancelPlacingBuilding();
            }
        }
    }
}