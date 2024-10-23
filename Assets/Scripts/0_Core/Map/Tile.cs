using _0_Core.Building;

namespace _0_Core.Map {
    public class Tile {
        public TileType TileType;
        public Building.Building Building;

        public Tile(TileType tileType, Building.Building building = null) {
            TileType = tileType;
            Building = building;
        }
    }
}