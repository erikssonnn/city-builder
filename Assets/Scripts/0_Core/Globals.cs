using System;
using _0_Core.Entity.Building;
using _0_Core.Entity.Terrain;
using _0_Core.Entity.Unit;

namespace _0_Core {
	public abstract class Globals {
		public static BuildingData[] BUILDING_DATA = {
			new BuildingData(Guid.NewGuid(), "house_1", 250, 3),
			new BuildingData(Guid.NewGuid(), "house_2", 650, 4)
		};

		public static TerrainData[] TERRAIN_DATA = {
			new TerrainData(Guid.NewGuid(), "tree_1", 50, 0, TerrainType.TREE, 50),
			new TerrainData(Guid.NewGuid(), "small_stone_1", 100, 0, TerrainType.STONE, 20)
		};

		public static UnitData[] UNIT_DATA = {
			new UnitData(Guid.NewGuid(), "unit_1", 10, 1, 5),
			new UnitData(Guid.NewGuid(), "unit_2", 20, 2, 3)
		};
		
		public static int TERRAIN_LAYER_MASK = 1 << 7;
		public static int MAP_SIZE = 256;
	}
}
