using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjects;
using Logger = erikssonn.Logger;
using Random = UnityEngine.Random;

public class PopulationType {
    public int workers;
    public int farmers;
    public int lumbermen;
    public int miners;

    public PopulationType(int workers, int farmers, int lumbermen, int miners) {
        this.workers = workers;
        this.farmers = farmers;
        this.lumbermen = lumbermen;
        this.miners = miners;
    }
}

public class PopulationController : MonoBehaviour {
    [Header("POPULATION: ")]
    [SerializeField] private int startPopulation = 0;
    [SerializeField] private int populationIncreaseCheckThreshold = 0;

    [SerializeField] private GameObject unitPrefab = null;
    [SerializeField] private Transform parentObj = null;

    public List<UnitController> AllUnits { get; set; } = new List<UnitController>();
    private int capacity = 0;
    private Color defaultTextColor = Color.clear;
    private UiController ui = null;
    private float populationIncreaseCheckTimer = 0.0f;

    public static PopulationController Instance { get; private set; }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start() {
        Logger.Print("HDHHDASHD");
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
            if (AllUnits.Count() < capacity && ResourceController.Instance.Food.amount > 0) {
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
        ui.populationAmountText.text = AllUnits.Count() + "/" + capacity;
        if (AllUnits.Count() > capacity) {
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
        UnitController unitObject = newUnit.GetComponent<UnitController>();
        AllUnits.Add(unitObject);
        UpdatePopulationText();
        MapController.Instance.CitySize = 10 + AllUnits.Count;
    }
}