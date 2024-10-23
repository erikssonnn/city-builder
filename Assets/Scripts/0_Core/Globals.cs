using _0_Core.Buildings;

namespace _0_Core {
	public abstract class Globals {
		public static BuildingData[] BUILDING_DATA = new BuildingData[] {
			new BuildingData("Building", 100)
		};

		public static int TERRAIN_LAYER_MASK = 1 << 7;
	}
}
