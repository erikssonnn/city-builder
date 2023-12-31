using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ScriptableObjects;
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
    [SerializeField] private List<Occupation> allOccupations = new List<Occupation>();

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
        unitObject.Occupation = allOccupations[0];
        AllUnits.Add(unitObject);
        UpdatePopulationText();
        UpdateOccupationsUi();
        MapController.Instance.CitySize = 10 + AllUnits.Count;
    }

    public void IncreaseOccupation(int index) {
        if (GetPopulationTypesAmount().workers == 0) return;
        UnitController unit = GetUnit(0);
        unit.Occupation = allOccupations[index];
        UpdateOccupationsUi();
    }
    
    public void DecreaseOccupation(int index) {
        if(index == 1)
            if (GetPopulationTypesAmount().farmers == 0) return;
        if(index == 2)
            if (GetPopulationTypesAmount().lumbermen == 0) return;
        if(index == 3)
            if (GetPopulationTypesAmount().miners == 0) return;
        
        UnitController unit = GetUnit(index);
        unit.Occupation = allOccupations[0];
        UpdateOccupationsUi();
    }

    private void UpdateOccupationsUi() {
        PopulationType populationType = GetPopulationTypesAmount();

        ui.workerAmountText.text = populationType.workers.ToString();
        ui.farmerAmountText.text = populationType.farmers.ToString();
        ui.lumbermanAmountText.text = populationType.lumbermen.ToString();
        ui.minerAmountText.text = populationType.miners.ToString();

        ui.decreaseFarmers.interactable = populationType.farmers > 0;
        ui.decreaseLumbermen.interactable = populationType.lumbermen > 0;
        ui.decreaseMiners.interactable = populationType.miners > 0;
        ui.increaseFarmers.interactable = populationType.workers > 0;
        ui.increaseLumbermen.interactable = populationType.workers > 0;
        ui.increaseMiners.interactable = populationType.workers > 0;
    }

    public PopulationType GetPopulationTypesAmount() {
        int workers = 0, farmers = 0, lumbermen = 0, miners = 0;
        // ReSharper disable once ConvertIfStatementToSwitchStatement (if statement is cleaner)
        foreach (UnitController unit in AllUnits) {
            if (unit.Occupation.index == 0)
                workers++;
            if (unit.Occupation.index == 1)
                farmers++;
            if (unit.Occupation.index == 2)
                lumbermen++;
            if (unit.Occupation.index == 3)
                miners++;
        }

        return new PopulationType(workers, farmers, lumbermen, miners);
    }

    private UnitController GetUnit(int index) {
        foreach (UnitController t in AllUnits.Where(t => t.Occupation.index == index)) {
            return t;
        }

        throw new System.Exception("No free unit was detected, please check before you run this function!");
    }
}