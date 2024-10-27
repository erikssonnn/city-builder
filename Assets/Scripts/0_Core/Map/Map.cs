using System.Collections.Generic;
using System.Linq;
using erikssonn;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _0_Core.Map {
    public static class Map {
        private static Dictionary<Vector2Int, Tile> _map = new Dictionary<Vector2Int, Tile>();

        public static void Clear() {
            _map.Clear();
        }
        
        public static void SetTile(Vector2Int position, Tile tile) {
            if (!IsFree(position)) {
                Logger.Print("Cant set tile, it is taken!", LogLevel.WARNING);
                return;
            }
            _map.Add(position, tile);
        }

        public static void SetTile(Vector2Int[] positions, Tile tile) {
            foreach (Vector2Int position in positions) {
                if (!IsFree(position)) {
                    Logger.Print("Cant set tile, it is taken!", LogLevel.WARNING);
                    return;
                }
                _map.Add(position, tile);
            }
        }
        
        public static bool IsFree(Vector2Int position) => _map.All(val => val.Key != position);
        public static int Size => _map.Count;
        public static Dictionary<Vector2Int, Tile> GetMap() => _map;
    }
}