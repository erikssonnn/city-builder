using System;
using System.Collections.Generic;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _0_Core.Building {
    public enum BuildingPlacement {
        VALID,
        INVALID,
        FIXED
    }

    /// <summary>
    /// Building object used as a container for the map and when creating buildings
    /// </summary>
    public class Building {
        private BuildingData _data;
        private Transform _transform;
        private Vector2Int[] _grid;
        private int _size;
        private Guid _id;
        private string _name;
        private BuildingPlacement _placement;

        private List<Material> _materials;
        private Material _validMaterial;
        private Material _invalidMaterial;

        public Building(BuildingData data) {
            _data = data;
            _placement = BuildingPlacement.VALID;
            _size = data.Size;
            _id = Guid.NewGuid(); // create new id per instance of building to track allocated map tiles
            _name = data.Name;
            _grid = new Vector2Int[(_size * 2 + 1) * (_size * 2 + 1)];

            Object[] prefabs = Resources.LoadAll($"Prefabs/Entity/Building/{_data.Name}");
            if (prefabs == null || prefabs.Length == 0) {
                Logger.Print($"Cant find {_data.Name} when creating building", LogLevel.FATAL);
            }

            GameObject gameObject = Object.Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;
            _transform = gameObject.transform;
            _transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            Rotate(Random.Range(-180, 180));

            // Materials
            _materials = new List<Material>();
            foreach (Material material in _transform.Find("mesh").GetComponent<MeshRenderer>().materials) {
                _materials.Add(new Material(material));
            }

            _validMaterial = Resources.Load($"Materials/valid") as Material;
            _invalidMaterial = Resources.Load($"Materials/invalid") as Material;
        }

        public void CheckValidPlacement() {
            foreach (Vector2Int pos in _grid) {
                if (!Map.Map.IsFree(pos) || !Map.Map.IsInsideMap(pos)) {
                    _placement = BuildingPlacement.INVALID;
                    break;
                }

                _placement = BuildingPlacement.VALID;
            }
        }

        public void Place() {
            _transform.localScale = new Vector3(1f, 1f, 1f);
            _placement = BuildingPlacement.FIXED;
            SetMaterial();
        }

        public void Rotate(int value = 90) {
            _transform.Find("mesh").Rotate(new Vector3(0, value, 0), Space.World);
        }

        public void Destroy() {
            Object.Destroy(_transform.gameObject);
        }

        #region Set & Get

        public void SetPosition(Vector2Int position) {
            _transform.position = new Vector3(position.x, 0, position.y);
        }

        public void SetGrid(Vector2Int[] grid) {
            for (int i = 0; i < grid.Length; i++) {
                _grid[i] = grid[i];
            }
        }

        public void SetMaterial() {
            List<Material> materials;
            switch (_placement) {
                case BuildingPlacement.VALID: {
                    materials = new List<Material>();
                    for (int i = 0; i < _materials.Count; i++) {
                        materials.Add(_validMaterial);
                    }

                    break;
                }
                case BuildingPlacement.INVALID: {
                    materials = new List<Material>();
                    for (int i = 0; i < _materials.Count; i++) {
                        materials.Add(_invalidMaterial);
                    }

                    break;
                }
                case BuildingPlacement.FIXED:
                default:
                    materials = _materials;
                    break;
            }

            _transform.Find("mesh").GetComponent<MeshRenderer>().materials = materials.ToArray();
        }

        public bool HasValidPlacement => _placement == BuildingPlacement.VALID;
        public Transform Transform => _transform;
        public Vector2Int[] Grid => _grid;
        public bool IsFixed => _placement == BuildingPlacement.FIXED;
        public int Size => _size;
        public Guid Id => _id;
        public string Name => _name;

        #endregion
    }
}