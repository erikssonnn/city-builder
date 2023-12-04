using System;
using UnityEngine;

public class TimeController : MonoBehaviour {
    private bool paused = true;
    private UiController ui = null;
    
    private void Start() {
        ui = UiController.Instance;
        TogglePause();
    }

    public void TogglePause() {
        paused = !paused;
        Time.timeScale = paused ? 0.0f : 1.0f;
        ui.pauseImage.sprite = paused ? ui.playSprite : ui.pauseSprite;
    }
}
