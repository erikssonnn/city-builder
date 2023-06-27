using UnityEngine;

namespace ScriptableObjects {
	[CreateAssetMenu(menuName = "Custom/Resource", fileName = "new Resource", order = 1)]
	public class Resource : ScriptableObject {
		public new string name;
		public GameObject prefab;
		public int value = 100;
		public Vector3Int size;
		[Range(0.0f, 1.0f)]public float rarity = 0.5f;
		public bool randomRot = false;
	}
}
