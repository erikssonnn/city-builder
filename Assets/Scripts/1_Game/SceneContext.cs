using System.Collections.Generic;
using _0_Core.Map;
using UnityEngine;

namespace _1_Game {
    
    /// <summary>
    /// This is the sceneContext. On start of each scene this will run and will create critical objects such as camera etc
    /// </summary>
    public class SceneContext : MonoBehaviour {
        private void OnDrawGizmos() {
            if (Map.Size <= 0) {
                return;
            }
            Gizmos.color = Color.red;
            foreach (KeyValuePair<Vector2Int, Tile> entry in Map.GetMap()) {
                Gizmos.DrawCube(new Vector3(entry.Key.x, 5, entry.Key.y), Vector3.one);
            }
        }
    }
}