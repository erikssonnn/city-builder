using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace _0_Core.Buildings {
    public class Building {
        private BuildingData _data;
        private Transform _transform;
        private int _currentHealth;

        public Building(BuildingData data) {
            _data = data;
            _currentHealth = data.Health;
            
            GameObject gameObject = GameObject.Instantiate(Resources.Load($"Prefabs/Buildings/{_data.Name}")) as GameObject;
            if (gameObject == null) {
                Logger.Print($"Cant find {_data.Name} when creating building", LogLevel.FATAL);
                Debug.Break();
            }
            _transform = gameObject.transform;
        }

        public void SetPosition(Vector3 position) {
            _transform.position = position;
        }

        public string Name => _data.Name;
        public Transform Transform => _transform;
        public int Health {
            get => _currentHealth;
            set => _currentHealth = value;
        }
        public int MaxHealth => _data.Health;

        public int DataIndex {
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