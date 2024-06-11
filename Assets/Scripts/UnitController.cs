using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Mission;

[BurstCompile]
public class UnitController : MonoBehaviour {
    [SerializeField] private float speed = 0.0f;
    [SerializeField] private int frameUpdateFrequency = 6;

    private Camera cam = null;
    private MapController mapController = null;
    private Misison mission = null;
    private int frameTimer = 0;

    private void Start() {
        mapController = MapController.Instance;
        if (mapController == null) {
            throw new System.Exception("Could not find MapController");
        }

        cam = Camera.main;
        if (cam == null)
            throw new System.Exception("Could not find Main Camera!");
        RandomizeClothes();
        mission = new IdleMission(float3.zero, gameObject, speed, mapController);
    }

    private void Update() {
        if (mission == null) { return; }
        if (frameTimer % frameUpdateFrequency == 0) {
            mission.Tick();
        }
        frameTimer++;
    }

    private void RandomizeClothes() {
        MeshRenderer ren = GetComponentInChildren<MeshRenderer>();
        Material[] materials = ren.sharedMaterials;
        Material[] newMaterials = new Material[materials.Length];

        for (int i = 0; i < materials.Length; i++) {
            newMaterials[i] = new Material(materials[i]);
        }

        newMaterials[1].color = Random.ColorHSV();
        ren.materials = newMaterials;
    }
}