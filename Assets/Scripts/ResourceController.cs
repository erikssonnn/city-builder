using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour {
    private int Food { get; set; }
    private int Wood { get; set; }
    private int Stone { get; set; }
    private UiController ui = null;

    private void Start() {
        ui = UiController.Instance;
        ChangeResource(new BuildingCost(20, 20, 20));
        DisplayResourceCost(new BuildingCost(0, 0, 0));
    }

    public void ChangeResource(BuildingCost cost) {
        Food += cost.food;
        Wood += cost.wood;
        Stone += cost.stone;

        if (Food < 0) Food = 0;
        if (Wood < 0) Wood = 0;
        if (Stone < 0) Stone = 0;

        if(cost.food < 0)
            ShakeController.Instance.Shake(ui.foodAmountText.transform, 200.0f, 0.25f);
        if(cost.wood < 0)
            ShakeController.Instance.Shake(ui.woodAmountText.transform, 200.0f, 0.25f);
        if(cost.stone < 0)
            ShakeController.Instance.Shake(ui.stoneAmountText.transform, 200.0f, 0.25f);

        UpdateResourceUi();
    }

    public bool EnoughResources(BuildingCost cost) {
        return Mathf.Abs(cost.food) <= Food &&
               Mathf.Abs(cost.wood) <= Wood &&
               Mathf.Abs(cost.stone) <= Stone;
    }

    private void UpdateResourceUi() {
        ui.foodAmountText.text = Food.ToString("F0");
        ui.woodAmountText.text = Wood.ToString("F0");
        ui.stoneAmountText.text = Stone.ToString("F0");
    }

    public void DisplayResourceCost(BuildingCost cost) {
        ui.foodCostText.text = cost.food == 0 ? "" : cost.food.ToString("F0");
        ui.woodCostText.text = cost.wood == 0 ? "" : cost.wood.ToString("F0");
        ui.stoneCostText.text = cost.stone == 0 ? "" : cost.stone.ToString("F0");
    }
}