using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UnityEngine;

public class MapController : MonoBehaviour {
    [Header("MAIN: ")] [SerializeField] private Vector2Int mapSize = Vector2Int.zero;
    [SerializeField] private int resourceMultiplier = 1000;
    [SerializeField] private GameObject resourcesParent = null;

    [Header("DEBUG: ")] [SerializeField] private bool drawGizmos = false;

    private int[] resourcePoints = new int[3];

    public List<Vector3Int> Map { get; set; } = new List<Vector3Int>();
    public static MapController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Vector2Int MapSize {
        get => mapSize;
        set => mapSize = value;
    }

    private void OnDrawGizmos() {
        if (!drawGizmos) return;
        if (Map == null) return;

        Gizmos.color = Color.magenta;
        foreach (Vector3 center in Map.Select(position =>
                     new Vector3(position.x + 0.5f, 0.0f, position.z + 0.5f))) {
            Gizmos.DrawCube(center, Vector3.one);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(-MapSize.x, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, MapSize.y * 2.0f));
        Gizmos.DrawCube(new Vector3(MapSize.x, 0.0f, 0.0f), new Vector3(1.0f, 1.0f, MapSize.y * 2.0f));
        Gizmos.DrawCube(new Vector3(0.0f, 0.0f, -MapSize.y), new Vector3(MapSize.x * 2.0f, 1.0f, 1.0f));
        Gizmos.DrawCube(new Vector3(0.0f, 0.0f, MapSize.y), new Vector3(MapSize.x * 2.0f, 1.0f, 1.0f));
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

    private static bool IsInStartArea(IEnumerable<Vector3Int> obj) {
        return obj.Any(t => {
            Vector3 position = new Vector3(t.x, t.y, t.z);
            return (position.x >= -10 && position.x <= 10) && (position.z >= -10 && position.z <= 10);
        });
    }

    public bool IntersectsMapPos(IEnumerable<Vector3Int> obj) {
        return obj.Any(t => Map.Contains(t));
    }

    private void GenerateResources() {
        foreach (Resource res in ResourceController.Instance.Resources) {
            int amount = Mathf.RoundToInt(res.rarity * resourceMultiplier);
            for (int i = 0; i < amount; i++) {
                if (Random.value > res.rarity) continue;
                SpawnResource(res);
            }
        }

        ResourceController.Instance.ResourcesOnMap = resourcePoints;
        print(resourcePoints[1]);
    }

    private bool CanPlace(List<Vector3Int> positions) {
        if (positions.Count <= 0) return false;
        return !IntersectsMapPos(positions);
    }

    public int GetResourceAmountOnMap(int index) {
        return resourcePoints[index];
    }

    private void SpawnResource(Resource res) {
        Vector3Int randomPos = GetRandomPos();
        if (Map.Contains(randomPos)) {
            SpawnResource(res);
        }

        Quaternion rot = res.randomRot
            ? Quaternion.Euler(new Vector3(0, Random.Range(-180, 180), 0))
            : Quaternion.identity;
        GameObject newObj = Instantiate(res.prefab, randomPos, rot, resourcesParent.transform);

        ResourceObject resourceObject = newObj.GetComponent<ResourceObject>();
        if (resourceObject == null) {
            throw new System.Exception("Cant find ResourceObject on spawned obj!");
        }

        Transform resourceT = resourceObject.transform;
        List<Vector3Int> positions = resourceObject.GetResourcePositions(resourceT.position, resourceT.rotation);

        if (!CanPlace(positions) || IsInStartArea(positions)) {
            SpawnResource(res);
            Destroy(newObj);
            return;
        }

        List<Vector3Int> resourcePointPositions = new List<Vector3Int>();

        foreach (Vector3Int t in positions.Where(t => !Map.Contains(t))) {
            newObj.GetComponent<ResourceObject>().Positions.Add(t);
            resourcePointPositions.Add(t);
            Map.Add(t);
        }

        resourcePoints = resourcePoints.Select((value, index) => {
            if (res.index == index) {
                value++;
            }

            return value;
        }).ToArray();
    }
}