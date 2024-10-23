using System.Collections.Generic;
using System.Linq;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace _0_Core.Map {
    public static class Map {
        private static Dictionary<Vector2Int, Tile> MAP = new Dictionary<Vector2Int, Tile>();
        public static bool IsFree(Vector2Int position) => MAP.All(val => val.Key != position);

        public static void SetTile(Vector2Int position, Tile tile) {
            if (!IsFree(position)) {
                Logger.Print("Cant set tile, it is taken!", LogLevel.WARNING);
                return;
            }
            MAP.Add(position, tile);
        }
    }
}