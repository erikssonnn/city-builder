using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using UnityEngine;
using Logger = erikssonn.Logger;
using Random = UnityEngine.Random;
using Unity.Mathematics;

[BurstCompile]
public class Unit {
    public readonly GameObject gameObject;
    public float3 destination;
    
    public Unit(GameObject gameObject, float3 destination) {
        this.gameObject = gameObject;
        this.destination = destination;
    }
}

[BurstCompile]
public class PopulationController : MonoBehaviour {
    [Header("POPULATION: ")]
    [SerializeField] private int startPopulation = 0;
    [SerializeField] private int populationIncreaseCheckThreshold = 0;

    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform parentObj = null;
    
    [Header("UNITS: ")]
    [SerializeField] private float speed = 0.0f;

    private readonly Dictionary<Hash128, Unit> units = new Dictionary<Hash128, Unit>();
    private int capacity = 0;
    private Color defaultTextColor = Color.clear;
    private UiController ui = null;
    private float populationIncreaseCheckTimer = 0.0f;
    private MapController mapController = null;

    private void Start() {
        ui = UiController.Instance;
        mapController = MapController.Instance;
        defaultTextColor = ui.populationAmountText.color;
        NullObjectCheck();
        PlaceStartUnits();
    }

    private void NullObjectCheck() {
        if (unitPrefab == null) {
            throw new System.Exception("unitPrefab is null on " + name);
        }
        if (parentObj == null) {
            throw new System.Exception("parentObj is null on " + name);
        }
        if (ui.populationAmountText == null) {
            throw new System.Exception("populationText is null on " + name);
        }
    }

    private void LateUpdate() {
        IncreasePopulationChecker();
    }

    private void IncreasePopulationChecker() {
        populationIncreaseCheckTimer += Time.deltaTime;
        
        while (populationIncreaseCheckTimer > populationIncreaseCheckThreshold) {
            if (units.Count() < capacity && ResourceController.Instance.Food.amount > 0) {
                if (Random.value > 0.25f) {
                    IncreasePopulation();
                }
            }
            
            populationIncreaseCheckTimer = 0.0f;
        }
    }

    public void IncreasePopulation() {
        int citySize = MapController.Instance.CitySize;
        Vector3Int randomPos = new Vector3Int(Random.Range(-citySize, citySize), 0, Random.Range(-citySize, citySize));
        CreateUnit(randomPos);
        MessageController.Instance.CreateMessage("Population increased!");
    }

    private void PlaceStartUnits() {
        for (int i = 0; i < startPopulation; i++) {
            int citySize = MapController.Instance.CitySize;
            Vector3Int randomPos = new Vector3Int(Random.Range(-citySize, citySize), 0, Random.Range(-citySize, citySize));
            CreateUnit(randomPos);
        }

        UpdatePopulationText();
    }

    private void UpdatePopulationText() {
        ui.populationAmountText.text = units.Count() + "/" + capacity;
        if (units.Count() > capacity) {
            ui.populationAmountText.color = Color.red;
            return;
        }

        ui.populationAmountText.color = defaultTextColor;
        ShakeController.Instance.Shake(ui.populationAmountText.transform, 200.0f, 0.25f);
    }

    public void ChangeCapacity(int amount) {
        capacity += amount;
        if (capacity < 0)
            capacity = 0;
        UpdatePopulationText();
    }

    public void CreateUnit(Vector3 pos) {
        GameObject newUnit = Instantiate(unitPrefab, pos, Quaternion.identity, parentObj);
        Unit unit = new Unit(newUnit, GetRandomPos());
        units.Add(Hash128.Compute(units.Count), unit);
        UpdatePopulationText();
        MapController.Instance.CitySize = 10 + units.Count;
        RandomizeClothes(unit);
    }

    #region SeparatedUnitController
    private void Update() {
        if (units.Count == 0) { return; }

        foreach (KeyValuePair<Hash128, Unit> entry in units) {
            Unit unit = entry.Value;
            if (math.distance(unit.gameObject.transform.position, unit.destination) <= 0.5f) {
                unit.destination = GetRandomPos();
            }

            float3 unitPos = unit.gameObject.transform.position;
            
            float3 direction = math.normalize(unitPos - unit.destination);
            float distance = math.distance(unit.destination, unitPos);
            float3 newPosition = unitPos - direction * Time.deltaTime * speed;
            float3 maskedPosition = math.select(unitPos, newPosition, distance > 0.5f);

            unit.gameObject.transform.position = maskedPosition;
        }
    }

    private Vector3 GetRandomPos() {
        int size = mapController.CitySize;

        Vector3 randomPos = Random.insideUnitCircle * 10f;
        Vector3 ret = transform.position + new Vector3(randomPos.x, 0f, randomPos.y);

        ret.x = Mathf.Clamp(ret.x, -size, size);
        ret.z = Mathf.Clamp(ret.z, -size, size);

        return ret;
    }

    private void RandomizeClothes(Unit unit) {
        MeshRenderer ren = unit.gameObject.GetComponentInChildren<MeshRenderer>();
        Material[] materials = ren.sharedMaterials;
        Material[] newMaterials = new Material[materials.Length];

        for (int i = 0; i < materials.Length; i++) {
            newMaterials[i] = new Material(materials[i]);
        }

        newMaterials[1].color = Random.ColorHSV();
        ren.materials = newMaterials;
    }

    #endregion
}