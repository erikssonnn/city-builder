using System;
using _0_Core.Map;
using erikssonn;
using UnityEngine;
using Logger = erikssonn.Logger;
using Vector2Int = _0_Core.Class.Vector2Int;

namespace _1_Game {
    /// <summary>
    /// Simple MapController, doesnt actually control any of the map stuff
    /// Just a monobehaviour to visualize the size of the map
    /// </summary>
    public class MapController : MonoBehaviour {
        private readonly Vector2Int[] positions = {
            new Vector2Int(-Map.Size / 2, -Map.Size / 2),
            new Vector2Int(Map.Size / 2, Map.Size / 2),
            new Vector2Int(-Map.Size / 2, Map.Size / 2),
            new Vector2Int(Map.Size / 2, -Map.Size / 2)
        };

        private void OnEnable() {
            GameObject parent = new GameObject {
                name = "Map Corners"
            };

            for (int i = 0; i < 4; i++) {
                GameObject mapCorner = Instantiate(Resources.Load("Prefabs/Entity/Building/other/construction_pole"), parent.transform, false) as GameObject;
                if (mapCorner == null) {
                    Logger.Print("Cant find construction_pole in Resources", LogLevel.WARNING);
                    return;
                }

                mapCorner.transform.position = new Vector3(positions[i].x, 0, positions[i].y);
            }
        }
    }
}