using _0_Core.Entity.Building;
using _0_Core.Entity.Terrain;

namespace _0_Core.Map {
    public class Tile {
        public TileType TileType;
        public Building Building;
        public Terrain Terrain;

        public Tile(TileType tileType, Building building = null, Terrain terrain = null) {
            TileType = tileType;
            Building = building;
            Terrain = terrain;
        }
    }
}