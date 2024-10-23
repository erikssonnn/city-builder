using System.Collections.Generic;
using System.Linq;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;

namespace _0_Core.Map {
    public class Map {
        private static Dictionary<Vector2Int, Tile> MAP = new Dictionary<Vector2Int, Tile>();
        public bool IsFree(Vector2Int position) => MAP.All(val => val.Key != position);

        public void SetTile(Vector2Int position, Tile tile) {
            if (!IsFree(position)) {
                Logger.Print("Cant set tile, it is taken!", LogLevel.WARNING);
                return;
            }
            MAP.Add(position, tile);
        }
    }
}