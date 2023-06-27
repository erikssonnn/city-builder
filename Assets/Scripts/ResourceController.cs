using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ResourceUi {
    public Text foodText, woodText, stoneText;
    public Text foodCostText, woodCostText, stoneCostText;
}

public class ResourceController : MonoBehaviour {
    [SerializeField] private ResourceUi resourceUi = new ResourceUi();

    public int Food { get; private set; }

    public int Wood { get; private set; }

    public int Stone { get; private set; }

    private void Start() {
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

        UpdateResourceUi();
    }

    public bool EnoughResources(BuildingCost cost) {
        return Mathf.Abs(cost.food) <= Food &&
               Mathf.Abs(cost.wood) <= Wood &&
               Mathf.Abs(cost.stone) <= Stone;
    }

    private void UpdateResourceUi() {
        resourceUi.foodText.text = Food.ToString("F0");
        resourceUi.woodText.text = Wood.ToString("F0");
        resourceUi.stoneText.text = Stone.ToString("F0");
    }

    public void DisplayResourceCost(BuildingCost cost) {
        resourceUi.foodCostText.text = cost.food == 0 ? "" : cost.food.ToString("F0");
        resourceUi.woodCostText.text = cost.wood == 0 ? "" : cost.wood.ToString("F0");
        resourceUi.stoneCostText.text = cost.stone == 0 ? "" : cost.stone.ToString("F0");
    }
}