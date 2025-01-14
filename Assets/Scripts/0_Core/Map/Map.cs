using System.Collections.Generic;
using System.Linq;
using _0_Core.Entity.Building;
using _0_Core.Entity.Terrain;
using erikssonn;
using Logger = erikssonn.Logger;
using Random = System.Random;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _0_Core.Map {
    public static class Map {
        private static Dictionary<Vector2Int, Tile> _map = new Dictionary<Vector2Int, Tile>();

        /// <summary>
        /// Set position on the map to a building
        /// </summary>
        /// <param name="building"></param>
        public static void SetPosition(Vector2Int position, Tile tile) {
            if (!IsFree(position)) {
                Logger.Print("Cant set position, it is taken!", LogLevel.WARNING);
                return;
            }

            _map.Add(position, tile);
        }

        /// <summary>
        /// Set positions on the map to a building
        /// </summary>
        /// <param name="building"></param>
        public static void SetPositions(Vector2Int[] positions, Tile tile) {
            foreach (Vector2Int position in positions) {
                if (!IsFree(position)) {
                    Logger.Print("Cant set position, it is taken!", LogLevel.WARNING);
                    return;
                }

                _map.Add(position, tile);
            }
        }

        /// <summary>
        /// Get a random position from the map that is not taken using a grid of positions
        /// </summary>
        /// <returns></returns>
        public static Vector2Int? GetRandomFreePosition() {
            Random random = new Random();
            HashSet<Vector2Int> attemptedPositions = new HashSet<Vector2Int>();
            int maxAttempts = (Size + 1) * (Size + 1);

            while (attemptedPositions.Count < maxAttempts) {
                int x = random.Next(-Size / 2, Size / 2 + 1);
                int y = random.Next(-Size / 2, Size / 2 + 1);
                Vector2Int position = new Vector2Int(x, y);

                if (attemptedPositions.Contains(position)) {
                    continue;
                }
                
                attemptedPositions.Add(position);

                if (IsFree(position) && IsInsideMap(position)) {
                    return position;
                }
            }

            return null;
        }

        public static void DeleteBuildingPositions(Building building) => building.Grid.ToList().ForEach(t => _map.Remove(t));
        public static void DeleteTerrainPositions(Terrain terrain) => terrain.Grid.ToList().ForEach(t => _map.Remove(t));
        public static Building GetBuildingFromPosition(Vector2Int position) => _map.Where(tile => tile.Key.Equals(position)).Select(tile => tile.Value.Building).FirstOrDefault();
        public static Terrain GetTerrainFromPosition(Vector2Int position) => _map.Where(tile => tile.Key.Equals(position)).Select(tile => tile.Value.Terrain).FirstOrDefault();
        public static bool IsFree(Vector2Int position) => _map.All(val => val.Key != position);

        public static bool IsInsideMap(Vector2Int position) =>
            position.x <= Size / 2 && position.x >= -Size / 2 && position.y <= Size / 2 && position.y >= -Size / 2;

        public static int Size => Globals.MAP_SIZE;
        public static Dictionary<Vector2Int, Tile> DebugMap() => _map; // only used for gizmos
        public static void Clear() => _map.Clear();
    }
}