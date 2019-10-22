using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static Vector2 ToVector2(this Vector3 vec3)
    {
        return new Vector2(vec3.x, vec3.y);
    }
}