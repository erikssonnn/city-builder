using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using UnityEngine;
using Logger = erikssonn.Logger;
using Random = UnityEngine.Random;

[BurstCompile]
public class PopulationController : MonoBehaviour {
    [Header("POPULATION: ")]
    [SerializeField] private int startPopulation = 0;
    [SerializeField] private int populationIncreaseCheckThreshold = 0;

    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform parentObj = null;

    private readonly Dictionary<Hash128, UnitController> units = new Dictionary<Hash128, UnitController>();
    private int capacity = 0;
    private Color defaultTextColor = Color.clear;
    private UiController ui = null;
    private float populationIncreaseCheckTimer = 0.0f;

    private void Start() {
        ui = UiController.Instance;
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
        units.Add(Hash128.Compute(units.Count), newUnit.GetComponent<UnitController>());
        UpdatePopulationText();
        MapController.Instance.CitySize = 10 + units.Count;
    }
}