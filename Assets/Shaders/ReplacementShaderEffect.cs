using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ReplacementShaderEffect : MonoBehaviour
{
    public Material replacementShader;
    public Camera cam;

    private void Update()
    {
        if (cam == null)
        {
            cam = transform.GetComponent<Camera>();
            cam.depthTextureMode = DepthTextureMode.Depth;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, replacementShader);
    }
}
