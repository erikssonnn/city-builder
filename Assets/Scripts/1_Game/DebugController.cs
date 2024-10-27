using System;
using System.Collections.Generic;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Random = UnityEngine.Random;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game {
    /// <summary>
    /// This is the sceneContext. On start of each scene this will run and will create critical objects such as camera etc
    /// </summary>
    public class DebugController : MonoBehaviour {
        private int size;
        
        private void OnValidate() {
            if (Map.Size % 2 != 0) {
                Logger.Print("Invalid map size, reverted to 64", LogLevel.WARNING);
                size = Mathf.RoundToInt(64 / 2.0f);
            }
            size = Mathf.RoundToInt(Map.Size / 2.0f);
        }

        private void OnDrawGizmos() {
            // grid
            Gizmos.color = Color.green;
            Vector3 centerOffset = new Vector3(-size / 2.0f, 0.1f, -size / 2.0f) + new Vector3(0.5f, 0, 0.5f);

            for (int x = 0; x <= size; x++) {
                Vector3 start = transform.position + new Vector3(x, 0.1f, 0) + centerOffset;
                Vector3 end = transform.position + new Vector3(x, 0.1f, size) + centerOffset;
                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= size; y++) {
                Vector3 start = transform.position + new Vector3(0, 0.1f, y) + centerOffset;
                Vector3 end = transform.position + new Vector3(size, 0.1f, y) + centerOffset;
                Gizmos.DrawLine(start, end);
            }
            
            // map
            foreach (KeyValuePair<Vector2Int, Tile> tile in Map.GetMap()) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(tile.Key.x, 0, tile.Key.y),  new Vector3(1f, 0.1f, 1f));
            }
        }

        private void OnGUI() {
            GUI.Label(new Rect(10, 10, 100, 100), $"Map size: {size}");
        }
    }
}