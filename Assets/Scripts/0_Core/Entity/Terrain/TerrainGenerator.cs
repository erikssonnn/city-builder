using _0_Core.Class;
using _0_Core.Map;
using Logger = erikssonn.Logger;
using LogLevel = erikssonn.LogLevel;

namespace _0_Core.Entity.Terrain {
    /// <summary>
    /// Generates the terrain
    /// </summary>
    public static class TerrainGenerator {
        private const int TRIES = 100;
        private static int _tries;

        public static void Generate() {
            foreach (TerrainData data in Globals.TERRAIN_DATA) {
                for (int a = 0; a < data.Weight; a++) {
                    _tries = 0;
                    Terrain terrain = new Terrain(data);
                    
                    if(TryPlaceTerrain(terrain)) {
                        _tries = 0;
                        Map.Map.SetPositions(terrain.Grid, new Tile(TileType.TERRAIN, null));
                    } else {
                        terrain.Destroy();
                        Logger.Print($"Tries to place {terrain.Name} but could not find a suitable place for it!", LogLevel.WARNING);
                    }
                }
            }
        }

        private static bool TryPlaceTerrain(Terrain terrain) {
            Vector2Int? position = Map.Map.GetRandomFreePosition();
            if (position == null) {
                Logger.Print($"Tried to generate {terrain.Name} but found no free positions", LogLevel.FATAL);
                return false;
            }

            terrain.SetPosition(position.Value);
            terrain.SetGrid(GetGridPositions(terrain, position.Value));
            if (!terrain.CanPlace()) {
                if (_tries >= TRIES) {
                    return false;
                }
                _tries++;
                TryPlaceTerrain(terrain);
                return false;
            }

            return true;
        }

        private static Vector2Int[] GetGridPositions(Terrain terrain, Vector2Int pos) {
            int totalPositions = (terrain.Size * 2 + 1) * (terrain.Size * 2 + 1);
            Vector2Int[] grid = new Vector2Int[totalPositions];

            int index = 0;
            for (int x = -terrain.Size; x <= terrain.Size; x++) {
                for (int y = -terrain.Size; y <= terrain.Size; y++) {
                    Vector2Int gridPos = new Vector2Int(pos.x + x, pos.y + y);
                    grid[index++] = gridPos;
                }
            }

            return grid;
        }
    }
}