using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects {
    public enum BuildingType { DEFAULT }

    [System.Serializable] 
    public class BuildingCost {
        public int food;
        public int wood;
        public int stone;

        public BuildingCost (int food, int wood, int stone) {
            this.food = food;
            this.wood = wood;
            this.stone = stone;
        }
    }

    [CreateAssetMenu(menuName = "Custom/Building", fileName = "new Building", order = 0)]
    public class Building : ScriptableObject {
        [Header("MAIN: ")]
        public int index;
        public new string name;
        public Sprite icon;
        public BuildingType buildingType;
        [FormerlySerializedAs("buildingSize")] public Vector3Int size;
        [FormerlySerializedAs("buildingPrefab")] public GameObject prefab;
        public BuildingCost cost;
    }
}