using System.Collections.Generic;
using _0_Core.Map;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _1_Game {
    /// <summary>
    /// This is the sceneContext. On start of each scene this will run and will create critical objects such as camera etc
    /// </summary>
    public class DebugController : MonoBehaviour {
        [Header("Grid Settings")]
        [SerializeField] private Vector2 minSize = new Vector2(5, 5);
        [SerializeField] private Vector2 maxSize = new Vector2(20, 20);
        [SerializeField] private float tileSize = 1.0f;

        private readonly Vector3 OFFSET = new Vector3(0.5f, 0.0f, 0.5f);        
        private int gridWidth;
        private int gridHeight;
        
        private void OnValidate() {
            gridWidth = Mathf.FloorToInt(Random.Range(minSize.x, maxSize.x) / tileSize);
            gridHeight = Mathf.FloorToInt(Random.Range(minSize.y, maxSize.y) / tileSize);
        }

        private void OnDrawGizmos() {
            // grid
            Gizmos.color = Color.green;

            for (int x = 0; x <= gridWidth; x++) {
                Vector3 start = transform.position + new Vector3(x * tileSize, 0.1f, 0) + OFFSET;
                Vector3 end = transform.position + new Vector3(x * tileSize, 0.1f, gridHeight * tileSize) + OFFSET;
                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= gridHeight; y++) {
                Vector3 start = transform.position + new Vector3(0, 0.1f, y * tileSize) + OFFSET;
                Vector3 end = transform.position + new Vector3(gridWidth * tileSize, 0.1f, y * tileSize) + OFFSET;
                Gizmos.DrawLine(start, end);
            }
            
            // map
            foreach (KeyValuePair<Vector2Int, Tile> tile in Map.GetMap()) {
                Gizmos.color = Color.red;
                Gizmos.DrawCube(new Vector3(tile.Key.x, 0, tile.Key.y),  new Vector3(1f, 0.1f, 1f));
            }
        }
    }
}