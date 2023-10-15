using UnityEngine.UI;
using UnityEngine;

public class MessageController : MonoBehaviour {
    [SerializeField] private float fadeOutThreshold = 5.0f;

    public static MessageController Instance { get; private set; }

    private float fadeOutTimer = 0.0f;
    private bool manuallyOpened = false;
    private UiController ui = null;

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
        ui.messagePanel.SetActive(false);
        manuallyOpened = false;
        fadeOutTimer = fadeOutThreshold;
    }

    public void CreateMessage(string message) {
        fadeOutTimer = 0.0f;
        ui.messageFeedText.text = message + "\n" + ui.messageFeedText.text;
    }

    public void ToggleMessagePanel() {
        manuallyOpened = !manuallyOpened;
        ui.messagePanel.SetActive(manuallyOpened);
        ui.messagePlusImage.sprite = manuallyOpened ? ui.minusSprite : ui.plusSprite;
    }

    private void Update() {
        fadeOutTimer += Time.deltaTime;

        if (manuallyOpened) {
            return;
        }

        ui.messagePanel.SetActive(!(fadeOutTimer > fadeOutThreshold));
    }
}