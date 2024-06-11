using System;
using DG.Tweening;
using UnityEngine;
using Logger = erikssonn.Logger;

public class MessageController : MonoBehaviour {
    [SerializeField] private RectTransform fadeoutObject = null;

    public static MessageController Instance { get; private set; }

    private bool open = false;
    private UiController ui = null;
    private Sequence tweenSequence = null;
    private float offscreenValue= 0.0f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        offscreenValue = -Screen.width;
        ui = UiController.Instance;
        ui.messagePanel.SetActive(false);
        fadeoutObject.anchoredPosition = new Vector2(offscreenValue, fadeoutObject.anchoredPosition.y);
        open = false;
    }

    public void CreateMessage(string message) {
        string str = message + "\n" + ui.messageFeedText.text;
        ui.messageFeedText.text = str;
        ui.popupText.text = str;
        if (open) { return; }
        ShowPopup();
    }

    public void ToggleMessagePanel() {
        open = !open;
        HidePopup();
        ui.messagePanel.SetActive(open);
        ui.messagePlusImage.sprite = open ? ui.minusSprite : ui.plusSprite;
    }

    private void ShowPopup() {
        tweenSequence.Kill();
        fadeoutObject.anchoredPosition = new Vector2(offscreenValue, fadeoutObject.anchoredPosition.y);
        tweenSequence = DOTween.Sequence();
        tweenSequence.Append(fadeoutObject.DOAnchorPosX(ui.messageButtonRectTransform.anchoredPosition.x, 0.8f));
        tweenSequence.AppendInterval(2.0f);
        tweenSequence.Append(fadeoutObject.DOAnchorPosX(offscreenValue, 0.8f));
        tweenSequence.Play();
    }

    private void HidePopup() {
        tweenSequence.Kill();
        fadeoutObject.anchoredPosition = new Vector2(offscreenValue, fadeoutObject.anchoredPosition.y);
    }
}