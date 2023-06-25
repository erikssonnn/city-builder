using System;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{
    [SerializeField] private Material imageEffectMaterial;
    [SerializeField] private int blockCount;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        // Pixelate calculations
        if (Camera.main == null)
            throw new Exception("Main Camera is null!");

        float k = Camera.main.aspect;
        Vector2 count = new Vector2(blockCount, blockCount / k);
        Vector2 size = new Vector2(1.0f / count.x, 1.0f / count.y);
        imageEffectMaterial.SetVector("block_count", count);
        imageEffectMaterial.SetVector("block_size", size);
        imageEffectMaterial.SetTexture("main_tex", src); // Set the source texture

        // Apply the shader
        Graphics.Blit(src, dst, imageEffectMaterial);
    }
}