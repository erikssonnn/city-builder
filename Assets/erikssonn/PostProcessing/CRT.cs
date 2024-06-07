using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class CRT : MonoBehaviour {
    public Material material;

    public int screenJumpFrequency = 1;
    public float screenJumpLength = 0.2f;
    public float screenJumpMinLevel = 0.1f;
    public float screenJumpMaxLevel = 0.9f;
    private float screenJumpTimeLeft;

    public float flickeringStrength = 0.002f;
    public float flickeringCycle = 111f;

    public bool isSlippage = true;
    public bool isSlippageNoise = true;
    public float slippageStrength = 0.005f;
    public float slippageInterval = 1f;
    public float slippageScrollSpeed = 33f;
    public float slippageSize = 11f;

    public float chromaticAberrationStrength = 0.005f;
    public bool isChromaticAberration = true;

    public bool isMultipleGhost = true;
    public float multipleGhostStrength = 0.01f;

    public bool isMonochrome = false;

    public bool isLowResolution = true;

    #region Shader properties
    private int _MonochormeOnOff;
    private int _ScreenJumpLevel;
    private int _FlickeringStrength;
    private int _FlickeringCycle;
    private int _SlippageStrength;
    private int _SlippageSize;
    private int _SlippageInterval;
    private int _SlippageScrollSpeed;
    private int _SlippageNoiseOnOff;
    private int _SlippageOnOff;
    private int _ChromaticAberrationStrength;
    private int _ChromaticAberrationOnOff;
    private int _MultipleGhostOnOff;
    private int _MultipleGhostStrength;
    private int _LetterBoxEdgeBlurOnOff;
    #endregion

    private void Start() {
        _MonochormeOnOff = Shader.PropertyToID("_MonochormeOnOff");
        _ScreenJumpLevel = Shader.PropertyToID("_ScreenJumpLevel");
        _FlickeringStrength = Shader.PropertyToID("_FlickeringStrength");
        _FlickeringCycle = Shader.PropertyToID("_FlickeringCycle");
        _SlippageStrength = Shader.PropertyToID("_SlippageStrength");
        _SlippageSize = Shader.PropertyToID("_SlippageSize");
        _SlippageInterval = Shader.PropertyToID("_SlippageInterval");
        _SlippageScrollSpeed = Shader.PropertyToID("_SlippageScrollSpeed");
        _SlippageNoiseOnOff = Shader.PropertyToID("_SlippageNoiseOnOff");
        _SlippageOnOff = Shader.PropertyToID("_SlippageOnOff");
        _ChromaticAberrationStrength = Shader.PropertyToID("_ChromaticAberrationStrength");
        _ChromaticAberrationOnOff = Shader.PropertyToID("_ChromaticAberrationOnOff");
        _MultipleGhostOnOff = Shader.PropertyToID("_MultipleGhostOnOff");
        _MultipleGhostStrength = Shader.PropertyToID("_MultipleGhostStrength");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetInt(_MonochormeOnOff, isMonochrome ? 1 : 0);
        material.SetFloat(_FlickeringStrength, flickeringStrength);
        material.SetFloat(_FlickeringCycle, flickeringCycle);
        material.SetFloat(_ChromaticAberrationStrength, chromaticAberrationStrength);
        material.SetInt(_ChromaticAberrationOnOff, isChromaticAberration ? 1 : 0);
        material.SetInt(_MultipleGhostOnOff, isMultipleGhost ? 1 : 0);
        material.SetFloat(_MultipleGhostStrength, multipleGhostStrength);
        material.SetInt(_SlippageOnOff, isSlippage ? 1 : 0);
        material.SetFloat(_SlippageInterval, slippageInterval);
        material.SetFloat(_SlippageNoiseOnOff, isSlippageNoise ? Random.Range(0, 1f) : 1);
        material.SetFloat(_SlippageScrollSpeed, slippageScrollSpeed);
        material.SetFloat(_SlippageStrength, slippageStrength);
        material.SetFloat(_SlippageSize, slippageSize);

        // Screen jump
        screenJumpTimeLeft -= 0.01f;
        if (screenJumpTimeLeft <= 0) {
            if (Random.Range(0, 1000) < screenJumpFrequency) {
                float level = Random.Range(screenJumpMinLevel, screenJumpMaxLevel);
                material.SetFloat(_ScreenJumpLevel, level);
                screenJumpTimeLeft = screenJumpLength;
            } else {
                material.SetFloat(_ScreenJumpLevel, 0);
            }
        }

        // Low Resolution
        if (isLowResolution) {
            RenderTexture target = RenderTexture.GetTemporary(source.width / 2, source.height / 2);
            Graphics.Blit(source, target);
            Graphics.Blit(target, destination, material);
            RenderTexture.ReleaseTemporary(target);
        } else {
            Graphics.Blit(source, destination, material);
        }
    }
}