using System.Collections.Generic;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace _0_Core.Building {
    public enum BuildingPlacement {
        VALID,
        INVALID,
        FIXED
    }
    
    public class Building {
        private BuildingData _data;
        private Transform _transform;
        private Vector3Int _position;
        private int _currentHealth;
        private BuildingPlacement _placement;
        private List<Material> _materials;

        public Building(BuildingData data) {
            _data = data;
            _currentHealth = data.Health;
            _placement = BuildingPlacement.VALID;
            
            GameObject gameObject = Object.Instantiate(Resources.Load($"Prefabs/Buildings/{_data.Name}")) as GameObject;
            if (gameObject == null) { Logger.Print($"Cant find {_data.Name} when creating building", LogLevel.FATAL); }
            _transform = gameObject.transform;
            _transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);

            _materials = new List<Material>();
            foreach (Material material in _transform.GetComponent<MeshRenderer>().materials) {
                _materials.Add(new Material(material));
            }
        }

        public void SetPosition(Vector3Int position) {
            _position = position;
            _transform.position = position;
        }

        public void SetMaterial() {
            List<Material> materials;
            switch (_placement) {
                case BuildingPlacement.VALID: {
                    Material refMaterial = Resources.Load($"Materials/valid") as Material;
                    materials = new List<Material>();
                    for (int i = 0; i < _materials.Count; i++)
                    {
                        materials.Add(refMaterial);
                    }
                    break;
                }
                case BuildingPlacement.INVALID: {
                    Material refMaterial = Resources.Load($"Materials/invalid") as Material;
                    materials = new List<Material>();
                    for (int i = 0; i < _materials.Count; i++)
                    {
                        materials.Add(refMaterial);
                    }
                    break;
                }
                case BuildingPlacement.FIXED:
                default:
                    materials = _materials;
                    break;
            }
            _transform.GetComponent<MeshRenderer>().materials = materials.ToArray();
        }

        public void Place() {
            _transform.localScale = new Vector3(1f, 1f, 1f);
            _placement = BuildingPlacement.FIXED;
            _transform.GetComponent<BoxCollider>().isTrigger = false;
            SetMaterial();
        }

        public void CheckValidPlacement(Vector3Int position) {
            if (_placement == BuildingPlacement.FIXED) { return; }
            _placement = Map.Map.IsFree(new Vector2Int(position.x, position.z))
                ? BuildingPlacement.VALID
                : BuildingPlacement.INVALID;
        }

        public bool HasValidPlacement => _placement == BuildingPlacement.VALID;
        public bool IsFixed => _placement == BuildingPlacement.FIXED;
        public string Name => _data.Name;
        public Transform Transform => _transform;
        public Vector3Int Position => _position;
        public int Health {
            get => _currentHealth;
            set => _currentHealth = value;
        }
        public int MaxHealth => _data.Health;

        public int BuildingIndex {
            get {
                for (int i = 0; i < Globals.BUILDING_DATA.Length; i++) {
                    if (Globals.BUILDING_DATA[i].Name == _data.Name) {
                        return i;
                    }
                }

                return -1;
            }
        }
    }
}