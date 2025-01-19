using _0_Core;
using _0_Core.Entity.Unit;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game._Entity._Unit {
    /// <summary>
    /// Controls when and how to create and control units in the world
    /// </summary>
    public class UnitController : MonoBehaviour {
        private static UnitController _instance;
        private Unit _currentUnit;
        private Camera _cam;
        private Vector2Int _mousePos;

        public static UnitController Instance {
            get {
                if (_instance == null) {
                    Logger.Print("UnitController is NULL", LogLevel.FATAL);
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
        }

        private void OnDestroy() { }

        private void Update() {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Globals.TERRAIN_LAYER_MASK)) {
                _mousePos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
            }
        }

        public void PlaceUnit(int unitIndex) {
            Unit unit = new Unit(Globals.UNIT_DATA[unitIndex]);
            unit.SetPosition(_mousePos);
        }
    }
}