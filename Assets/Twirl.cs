using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twirl : MonoBehaviour
{
    [ExecuteInEditMode]

    public Vector2 radius = new Vector2(0.3f, 0.3f);

    public Vector2 center = new Vector2(0.5f, 0.5f);

    [Range(0.0f, 360.0f)]
    public float angle = 0.0f;

    public Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0, 0, angle), Vector3.one);

        material.SetMatrix("_RotationMatrix", rotationMatrix);
        material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));

        Graphics.Blit(source, destination, material);
    }
}
