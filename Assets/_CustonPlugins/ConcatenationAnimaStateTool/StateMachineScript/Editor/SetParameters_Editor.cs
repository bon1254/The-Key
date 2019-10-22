using UnityEngine;
using UnityEditor;
using EditorTools;
using EditorTools.GUIStyles;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Animations;
namespace Funcy.ConcatenationAnimaStateTool
{
    [CustomEditor(typeof(SetParameters))]
    public class SetParameters_Editor : Editor
    {
        SetParameters data;

        private void OnEnable()
        {
            data = target as SetParameters;
        }
        public override void OnInspectorGUI()
        {
            parameterInfoControl();

            if (GUI.changed) SaveLayout();
        }
        void SaveLayout()
        {
            EditorUtility.SetDirty(target);
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
        private void parameterInfoControl()
        {
            var controller = (AnimatorController)data.targetController;
            if (data.parameterInfo.Count > 0)
            {
                int index = 0;
                foreach (var p in data.parameterInfo)
                {
                    string appearValue =
                       ((p.VariableType == AnimatorControllerParameterType.Bool) ? " , Value = " + p.boolValue.ToString() :
                       (p.VariableType == AnimatorControllerParameterType.Float) ? " , Value = " + p.floatValue.ToString() :
                       (p.VariableType == AnimatorControllerParameterType.Int) ? " , Value = " + p.intValue.ToString() : "");

                    if (EditorTool.DrawHeaderOnOff("#" + index.ToString("000") + "  " + p.parameterName
                       ,
                       InspectorGUIStyle.ShurikenModuleTitle
                       , "#" + index.ToString("000") + "  " + p.parameterName +
                       "       Type : " + p.VariableType.ToString() + appearValue
                       ))

                    {
                        EditorGUILayout.BeginVertical(InspectorGUIStyle.ShurikenModuleBg.ToString(), GUILayout.Height(60));
                        {
                            EditorGUILayout.BeginHorizontal();
                            {
                                EditorGUILayout.LabelField("Parameter Type:", GUILayout.Width(100));
                                p.VariableType = (AnimatorControllerParameterType)EditorGUILayout.EnumPopup(p.VariableType, GUILayout.Width(150));

                                GUILayout.FlexibleSpace();
                                if (GUILayout.Button("delete")) { data.parameterInfo.Remove(p); SaveLayout(); break; }
                            }
                            EditorGUILayout.EndHorizontal();
                            parameterInfo(p);

                        }
                        EditorGUILayout.EndVertical();
                    }
                    index++;

                }
            }

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Add Set Event", EditorTool.Size.Setsize(200, 30)))
                { data.parameterInfo.Add(new SetParameters.ParmeterInfo()); SaveLayout(); }
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
            EditorTool.Draw(InspectorGUIStyle.RL_DragHandle);
        }
        private void parameterInfo(SetParameters.ParmeterInfo p)
        {
            var controller = (AnimatorController)data.targetController;
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    GUILayout.Label("Select Parameter:", EditorTool.Size.Setsize(147, 15));
                    GUILayout.Label("Value:  ");

                    switch (p.VariableType)
                    {
                        case AnimatorControllerParameterType.Float:
                        case AnimatorControllerParameterType.Int:
                            p.AddMode = GUILayout.Toggle(p.AddMode, new Texture2D(1, 1));
                            GUILayout.Label("Add Mode");
                            break;
                    }

                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Wait time Enable");
                    p.EnableWaitTime = GUILayout.Toggle(p.EnableWaitTime, new Texture2D(1, 1));
                }
                EditorGUILayout.EndHorizontal();

                List<AnimatorControllerParameter> parameters = controller.parameters.ToList()
                   .FindAll(pa => pa.type == p.VariableType);
                if (parameters.Count > 0)
                {
                    List<string> parameterNames = new List<string>();
                    foreach (var pa in parameters) parameterNames.Add(pa.name);

                    EditorGUILayout.BeginHorizontal("box");
                    {
                        p.selectIndex = EditorGUILayout.Popup(p.selectIndex, parameterNames.ToArray(), EditorTool.Size.Setsize(150, 20));

                        try
                        { p.parameterName = parameterNames[p.selectIndex]; }
                        catch { p.selectIndex--; }

                        switch (p.VariableType)
                        {
                            case AnimatorControllerParameterType.Bool:
                                p.boolValue = EditorGUILayout.Toggle(p.boolValue, GUILayout.Width(50));
                                break;
                            case AnimatorControllerParameterType.Float:
                                p.floatValue = EditorGUILayout.FloatField(p.floatValue, GUILayout.Width(50));
                                break;
                            case AnimatorControllerParameterType.Int:
                                p.intValue = EditorGUILayout.IntField(p.intValue, GUILayout.Width(50));
                                break;
                            case AnimatorControllerParameterType.Trigger:
                                break;
                        }
                        GUILayout.FlexibleSpace();
                        if (p.EnableWaitTime)
                        {
                            GUILayout.Label("Time " + ((p.RandomLate) ? "range:" : "second:"));
                            if (!p.RandomLate)
                                p.WaitTime = EditorGUILayout.FloatField(p.WaitTime, GUILayout.Width(50));
                            else
                            {
                                p.RandomLateRange.x = EditorGUILayout.FloatField(p.RandomLateRange.x, GUILayout.Width(50));
                                p.RandomLateRange.y = EditorGUILayout.FloatField(p.RandomLateRange.y, GUILayout.Width(50));
                            }
                            p.RandomLate = GUILayout.Toggle(p.RandomLate, "Random");
                        }
                        else
                        {
                            EditorTool.Draw(InspectorGUIStyle.U2DDOTdragDot, EditorTool.Size.Setsize(30, 5));
                            GUILayout.Label("Wait time not Enable");
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal("box");
                    {
                        GUILayout.Label("Type: " + p.VariableType.ToString() + " not found");
                        EditorTool.Draw(InspectorGUIStyle.CN_EntryWarn);
                        GUILayout.FlexibleSpace();
                    }
                    EditorGUILayout.EndHorizontal();

                }
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginHorizontal();
            {
                p.SetOneShot = GUILayout.Toggle(p.SetOneShot, "Set it when the first time enter.");
                p.CancelInvokeWhenStateExit = GUILayout.Toggle(p.CancelInvokeWhenStateExit, "CancelInvoke when state Exit.");
            }
            EditorGUILayout.EndHorizontal();
            EditorTool.Draw(InspectorGUIStyle.RL_DragHandle);
        }

    }
   
}