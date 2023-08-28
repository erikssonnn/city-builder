using UnityEngine;

namespace ScriptableObjects {
	[CreateAssetMenu(menuName = "Custom/Resource", fileName = "new Resource", order = 1)]
	public class Resource : ScriptableObject {
		public int index;
		public new string name;
		public GameObject prefab;
		public float increment = 0.01f;
		public Vector3Int size;
		[Range(0.0f, 1.0f)]public float rarity = 0.5f;
		public bool randomRot = false;
	}
}
