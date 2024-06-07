using System;
using UnityEngine;
using Logger = erikssonn.Logger;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class PostProcessing : MonoBehaviour {
    [Header("Pixelate: ")]
    [SerializeField] private Vector2Int pixelSize = new Vector2Int(320, 420);

    [SerializeField] private Vector2Int screenSize = new Vector2Int(1080, 4400);

    [Header("ColorGrading: ")]
    [SerializeField] private int colorPrecision = 32;

    private Camera cam = null;
    private Material pixelateMat = null;
    private Material colorMat = null;

    private void Awake() {
        cam = Camera.main;
        if (cam == null) {
            throw new Exception("Main camera not found!");
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        pixelateMat = new Material(Shader.Find("Hidden/pixelate"));
        colorMat = new Material(Shader.Find("Hidden/colorgrading"));

        if (colorMat == null || pixelateMat == null) {
            Graphics.Blit(source, destination);
            Logger.Throw("Cant find one of the postprocessing shader materials");
            return;
        }

        RenderTexture tempTexture = RenderTexture.GetTemporary(source.width, source.height);

        // color grading
        colorMat.SetFloat("_Colors", colorPrecision);
        Graphics.Blit(source, tempTexture, colorMat);

        // pixelate
        pixelateMat.SetInt("_PixelSize", GetPixelSizeForScreen());
        Graphics.Blit(tempTexture, destination, pixelateMat);

        RenderTexture.ReleaseTemporary(tempTexture);
    }

    private int GetPixelSizeForScreen() {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float screenValue = Mathf.Sqrt(screenWidth * screenWidth + screenHeight * screenHeight);
        int strength = Mathf.RoundToInt(Mathf.Lerp(pixelSize.x, pixelSize.y, Normalize(screenValue, screenSize.x, screenSize.y)));
        return strength;
    }

    private static float Normalize(float value, float min, float max) {
        return Mathf.Clamp01((value - min) / (max - min));
    }
}