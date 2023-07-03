using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiController : MonoBehaviour {
    // This looks kind of bad in here but it makes it cleaner overall in the code

    [Header("TEXTS: ")] 
    public Text foodAmountText;
    public Text woodAmountText;
    public Text stoneAmountText;
    public Text foodCostText;
    public Text woodCostText;
    public Text stoneCostText;
    public Text populationAmountText;
    public Text selectedNameText;
    public Text workerAmountText;
    public Text lumbermanAmountText;
    public Text minerAmountText;

    [Header("PANELS: ")] 
    public GameObject selectPanel;
    public GameObject occupationPanel;

    [Header("BUTTONS: ")] 
    public Button increaseLumbermen;
    public Button increaseMiners;
    public Button decreaseLumbermen;
    public Button decreaseMiners;

    [Header("IMAGES: ")] 
    public Image selectImage;
    public Image plusImage;

    [Header("SPRITES: ")] 
    [SerializeField] private Sprite plusSprite = null;
    [SerializeField] private Sprite minusSprite = null;

    public static UiController Instance { get; private set; }

    private bool workPanelOnOff = false;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void ToggleExtraMenu() {
        workPanelOnOff = !workPanelOnOff;
        plusImage.sprite = workPanelOnOff ? minusSprite : plusSprite;
        occupationPanel.SetActive(workPanelOnOff);
    }
}