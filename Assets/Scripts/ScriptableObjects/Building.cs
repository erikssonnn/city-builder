using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects {
    [System.Serializable] 
    public class BuildingCost {
        public float food;
        public float wood;
        public float stone;

        public BuildingCost (float food, float wood, float stone) {
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
        public int capacity;
        [FormerlySerializedAs("buildingSize")] public Vector3Int size;
        [FormerlySerializedAs("buildingPrefab")] public GameObject prefab;
        public BuildingCost cost;
        public bool[] resourcePoint;
    }
}