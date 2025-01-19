using UnityEngine;
using erikssonn;
using Vector2Int = _0_Core.Class.Vector2Int;
using Logger = erikssonn.Logger;

namespace _0_Core.Entity.Unit {
    /// <summary>
    /// Unit object used as a container when creating and using units
    /// </summary>
    public class Unit {
        private UnitData _data;
        private System.Guid _id;
        private string _name;
        private Transform _transform;
        private int _damage;
        private int _speed;

        public Unit(UnitData data) {
            _data = data;
            _id = System.Guid.NewGuid(); // create new id per instance of building to track allocated map tiles
            _name = data.Name;
            _damage = data.Damage;
            _speed = data.Speed;

            Object prefab = Resources.Load($"Prefabs/Entity/Unit/{_data.Name}");
            if (prefab == null) {
                Logger.Print($"Cant find {_data.Name} when creating Unit", LogLevel.FATAL);
            }

            GameObject gameObject = Object.Instantiate(prefab) as GameObject;
            _transform = gameObject.transform;
            _transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            
            GameObject parent = GameObject.Find("map_units");
            if (parent == null) {
                Logger.Print("Cant find map_units", LogLevel.WARNING);
            }
            _transform.SetParent(parent.transform, false);
        }

        public void Destroy() {
            Object.Destroy(_transform.gameObject);
        }

        #region Set & Get

        public void SetPosition(Vector2Int position) {
            _transform.position = new Vector3(position.x, 0, position.y);
        }

        public Transform Transform => _transform;
        public int Damage => _damage;
        public int Speed => _speed;
        public System.Guid Id => _id;
        public string Name => _name;

        #endregion
    }
}