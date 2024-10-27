using System;
using _0_Core.Building;

namespace _0_Core {
	public abstract class Globals {
		public static BuildingData[] BUILDING_DATA = new BuildingData[] {
			new BuildingData(new Guid(), "Building", 100, 2)
		};

		public static int TERRAIN_LAYER_MASK = 1 << 7;
	}
}
