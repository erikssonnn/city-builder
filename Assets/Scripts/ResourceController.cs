using ScriptableObjects;
using UnityEngine;

public class ResourceWrapper {
    public float amount;
    public float increment;

    public ResourceWrapper(float amount, float increment) {
        this.amount = amount;
        this.increment = increment;
    }
}

public class ResourceController : MonoBehaviour {
    [SerializeField] private float incrementThreshold = 0.0f;
    [SerializeField] private Resource[] resources  = null;

    private UiController ui = null;
    private float incrementTimer = 0.0f;
    
    public Resource[] Resources {
        get => resources;
        set => resources = value;
    }

    public int[] ResourcesOnMap { private get; set; } = new int[3];

    public ResourceWrapper Food { get; set; }
    public ResourceWrapper Wood { get; set; }
    public ResourceWrapper Stone { get; set; }
    public static ResourceController Instance { get; private set; }

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
        InitializeResources();
        DisplayResourceCost(new BuildingCost(0, 0, 0));
    }

    private void InitializeResources() {
        Food = new ResourceWrapper(100, 0.0f);
        Wood = new ResourceWrapper(100, 0.0f);
        Stone = new ResourceWrapper(100, 0.0f);
        UpdateResourceUi();
    }

    public void ChangeResource(BuildingCost cost) {
        Food.amount += cost.food;
        Wood.amount += cost.wood;
        Stone.amount += cost.stone;

        if (Food.amount < 0) Food.amount = 0;
        if (Wood.amount < 0) Wood.amount = 0;
        if (Stone.amount < 0) Stone.amount = 0;

        if(cost.food < 0)
            ShakeController.Instance.Shake(ui.foodAmountText.transform, 200.0f, 0.25f);
        if(cost.wood < 0)
            ShakeController.Instance.Shake(ui.woodAmountText.transform, 200.0f, 0.25f);
        if(cost.stone < 0)
            ShakeController.Instance.Shake(ui.stoneAmountText.transform, 200.0f, 0.25f);

        UpdateResourceUi();
    }

    private void LateUpdate() {
        incrementTimer += Time.deltaTime;
        while (incrementTimer > incrementThreshold) {
            Food.increment = 10 * (ResourcesOnMap[0] * resources[0].increment);
            Wood.increment = 10 * (ResourcesOnMap[1] * resources[1].increment);
            Stone.increment = 10 * (ResourcesOnMap[2] * resources[2].increment);
            
            ChangeResource(new BuildingCost(Food.increment, Wood.increment, Stone.increment));
            if (Food.amount <= 0.0f) {
                MessageController.Instance.CreateMessage("Your citizens are starving!");
                // Fix a starving behaviour here please
            }
            incrementTimer = 0.0f;
        }
    }

    public bool EnoughResources(BuildingCost cost) {
        return Mathf.Abs(cost.food) <= Food.amount &&
               Mathf.Abs(cost.wood) <= Wood.amount &&
               Mathf.Abs(cost.stone) <= Stone.amount;
    }

    private void UpdateResourceUi() {
        ui.foodAmountText.text = Food.amount.ToString("F0");
        ui.woodAmountText.text = Wood.amount.ToString("F0");
        ui.stoneAmountText.text = Stone.amount.ToString("F0");
    }

    public void DisplayResourceCost(BuildingCost cost) {
        ui.foodAdditionalText.text = cost.food == 0 ? "" : "<color=red>" + cost.food.ToString("F0") + "</color>";
        ui.woodAdditionalText.text = cost.wood == 0 ? "" : "<color=red>" + cost.wood.ToString("F0") + "</color>";
        ui.stoneAdditionalText.text = cost.stone == 0 ? "" : "<color=red>" + cost.stone.ToString("F0") + "</color>";
    }

    public void DisplayResourceIncrement(int index) {
        switch (index) {
            case -1:
                ui.foodAdditionalText.text = "";
                ui.woodAdditionalText.text = "";
                ui.stoneAdditionalText.text = "";
                break;
            case 0:
                string foodColor = Food.increment >= 0.0f ? "<color=green>+" : "<color=red>";
                ui.foodAdditionalText.text = foodColor + Food.increment.ToString("F3") + "</color>";
                break;
            case 1:
                string woodColor = Food.increment >= 0.0f ? "<color=green>+" : "<color=red>";
                ui.woodAdditionalText.text = woodColor + Wood.increment.ToString("F3") + "</color>";
                break;
            case 2:
                string stoneColor = Food.increment >= 0.0f ? "<color=green>+" : "<color=red>";
                ui.stoneAdditionalText.text = stoneColor + Stone.increment.ToString("F3") + "</color>";
                break;
        }
    }
}