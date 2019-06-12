using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Replaces the draw shader of the Camera this is attached to. If no shader is preset, this script resets the camera to default.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraShaderReplace : MonoBehaviour
{
    public Shader shader;

    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (shader)
            cam.SetReplacementShader(shader, "");
        else
            cam.ResetReplacementShader();
    }
}
