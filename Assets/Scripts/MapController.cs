using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class MapController : MonoBehaviour {
    [SerializeField] private Vector2Int mapSize = Vector2Int.zero;
    [SerializeField] private int resourceMultiplier = 1000;
    [SerializeField] private GameObject resourcesParent = null;
    [SerializeField] private Resource[] resources = null;

    public Vector2Int MapSize {
        get => mapSize;
        set => mapSize = value;
    }

    public List<Vector3Int> Map { get; set; } = new List<Vector3Int>();

    private void OnDrawGizmos() {
        if (Map == null) return;

        Gizmos.color = Color.magenta;
        foreach (Vector3 center in Map.Select(position =>
                     new Vector3(position.x + 0.5f, 0.0f, position.z + 0.5f))) {
            Gizmos.DrawCube(center, Vector3.one);
        }
    }

    private void Start() {
        if (resourcesParent == null) {
            throw new System.Exception("ResourceParent is null!");
        }

        GenerateResources();
    }

    private Vector3Int GetRandomPos() {
        return new Vector3Int(Random.Range(-MapSize.x, MapSize.x), 0, Random.Range(-MapSize.y, MapSize.y));
    }

    public bool IntersectsMapPos(IEnumerable<Vector3Int> obj) {
        return obj.Any(t => Map.Contains(t));
    }

    private void GenerateResources() {
        foreach (Resource res in resources) {
            int amount = Mathf.RoundToInt(res.rarity * resourceMultiplier);
            for (int i = 0; i < amount; i++) {
                if (Random.value > res.rarity) continue;
                SpawnResource(res);
            }
        }
    }

    private bool CanPlace(List<Vector3Int> positions) {
        if (positions.Count <= 0) return false;
        return !IntersectsMapPos(positions);
    }

    private void SpawnResource(Resource res) {
        Vector3Int randomPos = GetRandomPos();
        if (Map.Contains(randomPos)) {
            SpawnResource(res);
        }

        Quaternion rot = res.randomRot ? 
            Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0)) : Quaternion.identity;
        GameObject newObj = Instantiate(res.prefab, randomPos, rot, resourcesParent.transform);
        
        ResourceObject resourceObject = newObj.GetComponent<ResourceObject>();
        if (resourceObject == null) {
            throw new System.Exception("Cant find ResourceObject on spawned obj!");
        }
        
        Transform resourceT = resourceObject.transform;
        List<Vector3Int> positions = resourceObject.GetResourcePositions(resourceT.position, resourceT.rotation);

        if (!CanPlace(positions)) {
            SpawnResource(res);
            Destroy(newObj);
            return;
        }
        
        foreach (Vector3Int t in positions.Where(t => !Map.Contains(t))) {
            newObj.GetComponent<ResourceObject>().Positions.Add(t);
            Map.Add(t);
        }
    }
}