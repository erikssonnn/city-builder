using System.Collections.Generic;
using System.Linq;
using erikssonn;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _0_Core.Map {
    public static class Map {
        private static Dictionary<Vector2Int, Building.Building> _map = new Dictionary<Vector2Int, Building.Building>();

        /// <summary>
        /// Set position on the map to a building
        /// </summary>
        /// <param name="building"></param>
        public static void SetPosition(Vector2Int position, Building.Building building) {
            if (!IsFree(position)) {
                Logger.Print("Cant set position, it is taken!", LogLevel.WARNING);
                return;
            }

            _map.Add(position, building);
        }

        /// <summary>
        /// Set positions on the map to a building
        /// </summary>
        /// <param name="building"></param>
        public static void SetPositions(Vector2Int[] positions, Building.Building building) {
            foreach (Vector2Int position in positions) {
                if (!IsFree(position)) {
                    Logger.Print("Cant set position, it is taken!", LogLevel.WARNING);
                    return;
                }

                _map.Add(position, building);
            }
        }

        /// <summary>
        /// Deletes all positions related to a building
        /// </summary>
        /// <param name="building"></param>
        public static void DeleteBuildingPositions(Building.Building building) {
            List<Vector2Int> positionsToDelete = (from tile in _map where tile.Value.Id == building.Id select tile.Key).ToList();
            foreach (Vector2Int t in positionsToDelete) {
                _map.Remove(t);
            }
        }

        public static Building.Building GetBuildingFromPosition(Vector2Int position) => _map.Where(tile => tile.Key.Equals(position)).Select(tile => tile.Value).FirstOrDefault();
        public static bool IsFree(Vector2Int position) => _map.All(val => val.Key != position);
        public static bool IsInsideMap(Vector2Int position) =>
            position.x <= Size / 2 && position.x >= -Size / 2 && position.y <= Size / 2 && position.y >= -Size / 2;
        public static int Size => 64;
        public static Dictionary<Vector2Int, Building.Building> DebugMap() => _map; // only used for gizmos
        public static void Clear() => _map.Clear();
    }
}