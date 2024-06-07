using Unity.Burst;
using Unity.Jobs;

namespace Jobs {
	[BurstCompile]
	public struct UnitMoveJob : IJobParallelFor {
		public void Execute(int index) {
			
		}
	}
}
