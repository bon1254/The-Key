using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using EditorTools;
namespace Funcy.ConcatenationAnimaStateTool
{
    [CustomEditor(typeof(ResetAllParameters))]
    public class ResetAllParametets_Editor : Editor
    {
        ResetAllParameters data;
        private void OnEnable()
        {
            data = target as ResetAllParameters;
        }
        public override void OnInspectorGUI()
        {
            AppearSelectParameterDropDown();            
            if (GUI.changed) SaveLayout();
        }
        void SaveLayout()
        {
            EditorUtility.SetDirty(target);
            UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        }
        
        private void AppearSelectParameterDropDown()
        {
            var controller = (AnimatorController)data.targetController;

            List<string> selectPar = new List<string>();
            foreach (var p in controller.parameters) selectPar.Add(p.name);
            GUILayout.Label("This script will initialize parameters.");
            GUILayout.Label("Type corresponds to value as: (Bool, Trigger) = false ,(Int, Float) = 0");
            GUILayout.Space(10);
            GUILayout.Label("select Igron target");
            GUILayout.BeginHorizontal();
            data.selectTarget = EditorGUILayout.Popup(data.selectTarget, selectPar.ToArray());
            if (GUILayout.Button("Add it"))
            {
                if (!data.IgronParameters.Contains(selectPar[data.selectTarget]))
                    data.IgronParameters.Add(selectPar[data.selectTarget]);
                SaveLayout();
            }

            GUILayout.EndHorizontal();
            if (data.IgronParameters.Count > 0)
            {
                GUILayout.Label("Igron list:");
                foreach (var s in data.IgronParameters)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(s);
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("delete"))
                    {
                        data.IgronParameters.Remove(s);
                        break;
                    }
                    GUILayout.EndHorizontal();
                }

            }
        }
    }
}