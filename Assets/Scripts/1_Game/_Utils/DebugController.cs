using System;
using System.Collections.Generic;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;
using CoreMap = _0_Core.Map.Map;

namespace _1_Game._Debug {
    /// <summary>
    /// This is the sceneContext. On start of each scene this will run and will create critical objects such as camera etc
    /// </summary>
    public class DebugController : MonoBehaviour {
        private int _size;
        private float _deltaTime = 0.0f;

        private void OnValidate() {
            if (Map.Size % 2 != 0) {
                Logger.Print("Invalid map size, reverted to 64", LogLevel.WARNING);
                _size = 64;
            }

            // here we dont half it since we start building the grid at 0 and not -size
            _size = Map.Size;
        }

        private void OnDrawGizmos() {
            // grid
            Gizmos.color = Color.green;
            Vector3 centerOffset = new Vector3(-_size / 2.0f, 0.1f, -_size / 2.0f) + new Vector3(0.5f, 0, 0.5f);

            for (int x = -1; x <= _size; x++) {
                Vector3 start = transform.position + new Vector3(x, 0.1f, -1) + centerOffset;
                Vector3 end = transform.position + new Vector3(x, 0.1f, _size) + centerOffset;
                Gizmos.DrawLine(start, end);
            }

            for (int y = -1; y <= _size; y++) {
                Vector3 start = transform.position + new Vector3(-1, 0.1f, y) + centerOffset;
                Vector3 end = transform.position + new Vector3(_size, 0.1f, y) + centerOffset;
                Gizmos.DrawLine(start, end);
            }

            // map
            foreach (KeyValuePair<Vector2Int, Tile> tile in CoreMap.DebugMap()) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(tile.Key.x, 0, tile.Key.y), new Vector3(1f, 0.1f, 1f));
            }
        }

        private void Update() {
            _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        }

        private void OnGUI() {
            string fps = (1.0f / _deltaTime).ToString("F0");
            GUI.Label(new Rect(10, 10, 100, 100), $"Map size: {_size}");
            GUI.Label(new Rect(100, 10, 100, 100), $"{fps} FPS");
        }
    }
}