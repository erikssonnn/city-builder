using System;
using System.Linq;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _0_Core.Entity.Terrain {
    /// <summary>
    /// Terrain object used as a container for the map and when spawning terrain
    /// </summary>
    public class Terrain {
        private Transform _transform;
        private Vector2Int[] _grid;
        private int _size;
        private Guid _id;
        private string _name;
        private TerrainData _data;

        public Terrain(TerrainData data) {
            _data = data;
            _size = data.Size;
            _name = data.Name;
            _id = Guid.NewGuid(); // create new id per instance of terrain to track allocated map tiles
            _grid = new Vector2Int[(_size * 2 + 1) * (_size * 2 + 1)];

            Object[] prefabs = Resources.LoadAll($"Prefabs/Entity/Terrain/{_data.Type}/{_data.Name}");
            if (prefabs == null || prefabs.Length == 0) {
                Logger.Print($"Cant find 'Prefabs/Entity/Terrain/{_data.Type}/{_data.Name}' when creating terrain", LogLevel.FATAL);
            }

            GameObject gameObject = Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;
            _transform = gameObject.transform;
            Rotate(Random.Range(-180, 180));
        }

        public void Destroy() {
            Object.Destroy(_transform.gameObject);
        }
        
        private void Rotate(int value = 90) {
            _transform.Find("mesh").Rotate(new Vector3(0, value, 0), Space.World);
        }

        #region Get & Set

        public void SetPosition(Vector2Int position) {
            _transform.position = new Vector3(position.x, 0, position.y);
        }

        public void SetGrid(Vector2Int[] grid) {
            for (int i = 0; i < grid.Length; i++) {
                _grid[i] = grid[i];
            }
        }

        public Transform Transform => _transform;
        public Vector2Int[] Grid => _grid;
        public string Name => _name;
        public int Size => _size;
        public Guid Id => _id;
        public bool CanPlace() => _grid.All(pos => Map.Map.IsFree(pos) && Map.Map.IsInsideMap(pos));

        #endregion
    }
}