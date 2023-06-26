using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour {
    private List<Vector3Int> map = new List<Vector3Int>();

    public List<Vector3Int> Map {
        get => map;
        set => map = value;
    }
    
    private void OnDrawGizmos() {
        if (map == null) return;
        
        Gizmos.color = Color.yellow;
        foreach (Vector3 center in map.Select(position => new Vector3(position.x + 0.5f, position.y + 0.5f, position.z + 0.5f))) {
            Gizmos.DrawCube(center, Vector3.one);
        }
    }

    public bool IntersectsMapPos(IEnumerable<Vector3Int> obj) {
        return obj.Any(t => map.Contains(t));
    }
}