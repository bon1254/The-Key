using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[InitializeOnLoad]
public class SF_TagLayer
{
    static SF_TagLayer()
    {
        EditorApplication.update += CreateLayer;
        CreateLayer();
    }
    //creates a new layer
    static void CreateLayer()
    {
        if (AllLayer.Contains("SF_EditorPreview")) return;

        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
        SerializedProperty layers = tagManager.FindProperty("layers");
        for (int i = 31; i >= 8; i--)
        {
            var prop = layers.GetArrayElementAtIndex(i);
            if (prop.stringValue == "")
            {
                prop.stringValue = "SF_EditorPreview"; break;
            }
        }

        tagManager.ApplyModifiedProperties();
    }

    public static List<string> AllLayer
    {
        get
        {
            var result = new List<string>();
            for (int i = 0; i < 32; i++)
            {
                result.Add(LayerMask.LayerToName(i));
            }
            result.RemoveAll(x => string.IsNullOrEmpty(x));
            return result;
        }
    }
}
