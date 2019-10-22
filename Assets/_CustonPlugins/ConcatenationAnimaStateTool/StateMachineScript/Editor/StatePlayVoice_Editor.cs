using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTools;
namespace Funcy.ConcatenationAnimaStateTool
{
   [CustomEditor(typeof(StatePlayVoice))]
   public class StatePlayVoice_Editor : Editor
   {
      public override void OnInspectorGUI()
      {
         var This = target as StatePlayVoice; ;
         GUILayout.Space(10);

         if (VoiceManager.This)
         {
            if (VoiceManager.This.voiceType.Count > 0)
            {
               if (GUILayout.Button("Add Play Event"))
               {
                  This.voiceInfoList.Add(new StatePlayVoice.VoiceInfo()
                  {
                     clip = null,
                     playTimelate = 0.0f
                  });
                  SaveLayout();
               }
                  
               VoiceInfoList(This);
            }
            else
            {
               GUILayout.Label("There is no Voice Type in VoiceManager Please");
               if (GUILayout.Button("Go to Add it"))
               {
                  Selection.activeGameObject = FindObjectOfType<VoiceManager>().gameObject;
               }
            }
         }
         else
         {
            GUILayout.Label("VoiceManager not Found Please");
            if (GUILayout.Button("Add VoiceManager"))
            {
               GameObject go = new GameObject("VoiceManager", typeof(VoiceManager));

            }

         }
         GUILayout.Space(10);
         if (GUI.changed) SaveLayout();            
      }
      void SaveLayout()
      {
         EditorUtility.SetDirty(target);
         UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
      }

      private void VoiceInfoList(StatePlayVoice This)
      {
         List<string> selectTypes = new List<string>();
         int i = 0;
         if (Application.isPlaying) return;
         if (VoiceManager.This.voiceType.Count > 0)
         {
            foreach (var type in VoiceManager.This.voiceType)
               if (type.Path != "")
                  selectTypes.Add(type.Path.Split('/')[type.Path.Split('/').Length - 1]);
         }
         else return;


         foreach (var info in This.voiceInfoList)
         {
            List<string> selectClips = new List<string>();

            var type = VoiceManager.This.voiceType[info.selectType];

            foreach (var clip in type.clipList) selectClips.Add(clip.clipName);

            if (EditorTool.DrawHeader("#Play Event " + i.ToString("00"), EditorTools.GUIStyles.InspectorGUIStyle.ShurikenModuleTitle))
            {
               GUILayout.BeginVertical(EditorTools.GUIStyles.InspectorGUIStyle.ShurikenModuleBg.ToString(), GUILayout.Height(60));
               {
                  EditorGUILayout.BeginHorizontal();
                  {
                     GUILayout.Label("Select Voice Type:");
                     info.selectType = EditorGUILayout.Popup(info.selectType, selectTypes.ToArray());
                  }
                  EditorGUILayout.EndHorizontal();
                  GUILayout.Space(10);
                  EditorGUILayout.BeginHorizontal();
                  {
                     GUILayout.Label("Select Clip:");
                     info.index = EditorGUILayout.Popup(info.index, selectClips.ToArray());
                     if (GUILayout.Button("delete"))
                     {
                        This.voiceInfoList.Remove(info);
                        break;
                     }
                  }
                  EditorGUILayout.EndHorizontal();
                  EditorGUILayout.BeginHorizontal("box");
                  {
                     GUILayout.Label("Loop"); info.loop = GUILayout.Toggle(info.loop, new Texture2D(1,1));
                     if (type.clipList.Count > 0)
                        info.clip = type.clipList[info.index].clip;
                     GUILayout.FlexibleSpace();
                     GUILayout.Label("Start late:");
                     info.playTimelate = EditorGUILayout.FloatField(info.playTimelate, EditorTool.Size.Setsize(30, 15));
                  }
                  EditorGUILayout.EndHorizontal();
                  if (info.loop)
                  {
                     EditorGUILayout.BeginHorizontal("box");
                     {
                        GUILayout.Label("fadeIn"); info.fadeIn = GUILayout.Toggle(info.fadeIn, new Texture2D(1,1));
                        if (info.fadeIn)
                        { GUILayout.Label("→ Speed"); info.fadeSpeedIn = EditorGUILayout.FloatField(info.fadeSpeedIn, EditorTool.Size.Setsize(30, 15)); }
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("fadeOut"); info.fadeOut = GUILayout.Toggle(info.fadeOut, new Texture2D(1,1));
                        if (info.fadeOut)
                        { GUILayout.Label("→ Speed"); info.fadeSpeedOut = EditorGUILayout.FloatField(info.fadeSpeedOut, EditorTool.Size.Setsize(30, 15)); }
                     }
                     EditorGUILayout.EndHorizontal();
                  }
                  
               }
               GUILayout.EndVertical();
               EditorTool.Draw(EditorTools.GUIStyles.InspectorGUIStyle.RL_DragHandle);
            }
            i++;
         }

      }
   }
}