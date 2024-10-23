using System.Collections.Generic;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace _0_Core.Buildings {
    public enum BuildingPlacement {
        VALID,
        FIXED
    }
    
    public class Building {
        private BuildingData _data;
        private Transform _transform;
        private int _currentHealth;
        private BuildingPlacement _placement;
        private List<Material> _materials;

        public Building(BuildingData data) {
            _data = data;
            _currentHealth = data.Health;
            _placement = BuildingPlacement.VALID;
            
            GameObject gameObject = Object.Instantiate(Resources.Load($"Prefabs/Buildings/{_data.Name}")) as GameObject;
            if (gameObject == null) {
                Logger.Print($"Cant find {_data.Name} when creating building", LogLevel.FATAL);
                Debug.Break();
            }
            _transform = gameObject.transform;

            _materials = new List<Material>();
            foreach (Material material in _transform.Find("Mesh").GetComponent<Renderer>().materials) {
                _materials.Add(new Material(material));
            }
        }

        public void SetPosition(Vector3 position) {
            _transform.position = position;
        }

        public void Place() {
            _placement = BuildingPlacement.FIXED;
            _transform.GetComponent<BoxCollider>().isTrigger = false;
        }

        public bool HasValidPlacement => _placement == BuildingPlacement.VALID;
        public bool IsFixed => _placement == BuildingPlacement.FIXED;
        public string Name => _data.Name;
        public Transform Transform => _transform;
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