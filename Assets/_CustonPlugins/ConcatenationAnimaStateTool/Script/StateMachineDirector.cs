using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Animations;
#endif
public class StateMachineDirector : ScriptableObject
{
    public static StateMachineDirector data;
    public List<RuntimeAnimatorController> controllerAssets = new List<RuntimeAnimatorController>();

#if UNITY_EDITOR
    [HideInInspector]public List<string> AllControllerAssetPath = new List<string>();
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(StateMachineDirector))][InitializeOnLoad]
public class StateMachineDirector_Editor : Editor
{
    static StateMachineDirector_Editor()
    {
        EditorApplication.CallbackFunction delayInit = null;
        delayInit = delegate
        {
            Create("StateMachineDirector", "Assets/_CustonPlugins/ConcatenationAnimaStateTool/Resources");

            CheckAndUpdateAllControllerAsset();

            EditorApplication.projectChanged += delegate { CheckAndUpdateAllControllerAsset(); };

            EditorApplication.update += delegate
            {
                if (!StateMachineDirector.data) return;

                foreach (var controller in StateMachineDirector.data.controllerAssets)
                {
                    foreach (var manager in ((AnimatorController)controller).GetBehaviours<StateMachineManager>())
                    {
                        manager.director = StateMachineDirector.data;
                        manager.targetController = controller;
                    }
                }
            };
            EditorApplication.delayCall -= delayInit;
        };
        EditorApplication.delayCall += delayInit;
    }
    static void CheckAndUpdateAllControllerAsset()
    {
        if (BuildPipeline.isBuildingPlayer) return;
        if (!StateMachineDirector.data)
            Create("StateMachineDirector", "Assets/_CustonPlugins/ConcatenationAnimaStateTool/Resources");

        if (StateMachineDirector.data.AllControllerAssetPath.Count != GetAllControllerAssetPath.Count)
        {
            StateMachineDirector.data.AllControllerAssetPath = GetAllControllerAssetPath;

            List<RuntimeAnimatorController> controllerAssets = new List<RuntimeAnimatorController>();
            foreach (var animatorPath in StateMachineDirector.data.AllControllerAssetPath)
            {
                RuntimeAnimatorController controller = AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(toAssetsPath(animatorPath));
                controllerAssets.Add(controller);
            }
            controllerAssets.Sort((a1, a2) => a1.GetInstanceID().CompareTo(a2.GetInstanceID()));
            StateMachineDirector.data.controllerAssets = controllerAssets;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

    }

    static void Create(string fileName, string assetPath)
    {
        if (!StateMachineDirector.data)
            StateMachineDirector.data = CreateCannotBeDeleteObject<StateMachineDirector>(fileName, assetPath);
        EditorApplication.projectChanged += delegate
        {
            if (BuildPipeline.isBuildingPlayer) return;
            if (!StateMachineDirector.data)
                StateMachineDirector.data = CreateCannotBeDeleteObject<StateMachineDirector>(fileName, assetPath);
        };
    }

    public static T CreateCannotBeDeleteObject<T>(string fileName, string assetPath = "Assets") where T : ScriptableObject
    {
        try
        {

            if (Resources.FindObjectsOfTypeAll<T>().Length == 0 || Resources.FindObjectsOfTypeAll<T>().ToList().Find(x => x.name == fileName) == null)
            {
                ScriptableObject asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, assetPath + "/" + fileName + ".asset");
            }
            EditorApplication.projectChanged += delegate
            {
                if (BuildPipeline.isBuildingPlayer) return;
                if (Resources.FindObjectsOfTypeAll<T>().Length == 0 || Resources.FindObjectsOfTypeAll<T>().ToList().Find(x => x.name == fileName) == null)
                {
                    ScriptableObject asset = ScriptableObject.CreateInstance<T>();
                    AssetDatabase.CreateAsset(asset, assetPath + "/" + fileName + ".asset");
                    Debug.Log(fileName + "曰 : " + "你刪不掉  (́◉◞౪◟◉‵). ヽ(́◕◞౪◟◕‵)ﾉ.");
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<T>(assetPath + "/" + fileName);
                }
            };

            return Resources.FindObjectsOfTypeAll<T>().ToList().Find(x => x.name == fileName);
        }
        catch
        {
            return null;
        }
    }

    public static List<string> GetAllControllerAssetPath
    {
        get
            {
            List<string> allFiles = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".controller")).ToList();
            return allFiles;
        }
    }

    public static string toAssetsPath(string path)
    {
        return path.Replace(Application.dataPath, "Assets").Replace("/", @"\");
    }
}
#endif
