using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace Mission {
	[BurstCompile]
	[System.Serializable]
	public class Misison {
		public float3 destination;
		public GameObject gameObject;

		public Misison(float3 destination, GameObject gameObject) {
			this.destination = destination;
			this.gameObject = gameObject;
		}

		public virtual void Tick() { }
	}
}
