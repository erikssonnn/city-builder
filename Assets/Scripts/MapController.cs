using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour {
    [SerializeField] private Vector2Int mapSize = Vector2Int.zero;
    [SerializeField] private List<Vector3Int> map = null;

    private void OnDrawGizmos() {
        if (map == null) return;

        Gizmos.color = Color.yellow;
        foreach (Vector3Int position in map) {
            Vector3 center = new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 0.5f);
            Gizmos.DrawCube(center, Vector3.one);
        }
    }

    public List<Vector3Int> Map {
        get => map;
        set => map = value;
    }

    public bool IntersectsMapPos(List<Vector3Int> obj) {
        for (int i = 0; i < obj.Count; i++) {
            if (map.Contains(obj[i])) return true;
        }

        return false;
    }
}