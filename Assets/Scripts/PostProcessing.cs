using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PostProcessing : MonoBehaviour {
    [SerializeField] private Material mat;
    [SerializeField] private int blockCount;
    [SerializeField] private bool linearPixelate;

    private Camera cam = null;

    private void Awake() {
        cam = Camera.main;
        if (cam == null) {
            throw new System.Exception("Main camera not found!");
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture dest) {
        if (Camera.main == null) {
            throw new Exception("Main Camera is null!");
        }

        if (linearPixelate) {
            float orthoSize = Camera.main.orthographicSize;
            float t = Mathf.InverseLerp(15, 40, orthoSize);
            blockCount = Mathf.RoundToInt(Mathf.Lerp(128, 192, t));
        }

        float k = Camera.main.aspect;
        Vector2 count = new Vector2(blockCount, blockCount / k);
        Vector2 size = new Vector2(1.0f / count.x, 1.0f / count.y);
        
        mat.SetVector("block_count", count);
        mat.SetVector("block_size", size);
        mat.SetTexture("main_tex", source);
        
        Graphics.Blit(source, dest, mat);
    }
}