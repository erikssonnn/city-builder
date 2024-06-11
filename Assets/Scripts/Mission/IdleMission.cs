using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Logger = erikssonn.Logger;
using Random = UnityEngine.Random;

namespace Mission {
	[BurstCompile]
	public class IdleMission : Misison {
		private readonly float speed = 0.0f;
		private readonly MapController mapController = null;
		private const float downscale = 0.1f;
		
		public IdleMission(float3 destination, GameObject gameObject, float speed, MapController mapController)
			: base(destination, gameObject) {
			this.speed = speed;
			this.mapController = mapController;
		}

		public override void Tick() {
			base.Tick();

			if (math.distance(gameObject.transform.position, destination) <= 0.5f) {
				destination = GetRandomPos();
			}

			float3 unitPos = gameObject.transform.position;
        
			float3 direction = math.normalize(unitPos - destination);
			float distance = math.distance(destination, unitPos);
			float3 newPosition = unitPos - direction * Time.fixedDeltaTime * speed * downscale;
			float3 maskedPosition = math.select(unitPos, newPosition, distance > 0.5f);

			gameObject.transform.position = maskedPosition;
		}

		private Vector3 GetRandomPos() {
			int size = mapController.CitySize;

			float2 random = Random.insideUnitCircle * 10f;
			float3 randomPos = new float3(random.x, random.y, 0);
			float3 ret = gameObject.transform.position + new Vector3(randomPos.x, 0f, randomPos.y);

			ret.x = Mathf.Clamp(ret.x, -size, size);
			ret.z = Mathf.Clamp(ret.z, -size, size);

			return ret;
		}
	}
}
