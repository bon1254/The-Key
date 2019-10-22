using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTools;
using EditorTools.GUIStyles;
using System.IO;
namespace Funcy.ConcatenationAnimaStateTool
{
   [CustomEditor(typeof(VoiceManager))]
   public class VoiceManager_Editor : Editor
   {
      public override void OnInspectorGUI()
      {
         VoiceManager This = (VoiceManager)target;
         GUILayout.Space(10);
         if (GUILayout.Button("Add Voice Type"))
         {
            This.voiceType.Add(
             new VoiceManager.VoiceType()
             );
            SaveLayout();
         }
            
         GUILayout.Space(10);
         try
         {
            foreach (var type in This.voiceType)
            {
               if (EditorTool.DrawHeader("VoiceType List", InspectorGUIStyle.ShurikenModuleTitle))
               {
                  EditorGUILayout.BeginVertical(InspectorGUIStyle.ShurikenModuleBg.ToString(), GUILayout.Height(10));
                  GUILayout.Label("Path");
                  EditorGUILayout.BeginHorizontal();
                  type.Path = EditorGUILayout.TextField(type.Path);
                  if (GUILayout.Button("Borwse Path"))
                  {
                     string getPath = EditorUtility.OpenFolderPanel("VoiceType floder",
                        type.Path == "" ?
                        Application.dataPath
                        :
                        type.Path,
                        "");

                     type.Path = getPath == "" ? type.Path : getPath;

                     int start = type.Path.IndexOf("Assets");
                     type.Path = type.Path.Substring(start);
                  }

                  else EditorGUILayout.EndHorizontal();
                  GUILayout.Space(10);
                  EditorGUILayout.BeginHorizontal();
                  {
                     GUILayout.Space(10);
                     if (EditorTool.DrawHeader("AudioClip List: " + type.Path.Split('/')[type.Path.Split('/').Length - 1], InspectorGUIStyle.ShurikenModuleTitle))
                     {
                        EditorGUILayout.EndHorizontal();
                        ClipListUpdate(This, type);
                     }
                     else EditorGUILayout.EndHorizontal();
                  }
                  EditorGUILayout.EndVertical();
               }
               EditorTool.Draw(EditorTools.GUIStyles.InspectorGUIStyle.RL_DragHandle);
            }
         }
         catch { }
         if (GUI.changed) SaveLayout(); 
      }
      void SaveLayout()
      {
         EditorUtility.SetDirty(target);
         UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
      }
      private void ClipListUpdate(VoiceManager This, VoiceManager.VoiceType type)
      {
         EditorGUILayout.BeginHorizontal();
         GUILayout.Space(20);
         EditorGUILayout.BeginHorizontal();
         {
            if (GUILayout.Button("LoadClips")) { LoadClips(type); UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty(); }
            if (GUILayout.Button("Delete Type", EditorTool.Size.Setsize(100, 25))) { This.voiceType.Remove(type); EditorGUILayout.EndHorizontal(); return; }
         }
         EditorGUILayout.EndHorizontal();
         EditorGUILayout.EndHorizontal();
         int i = 0;
         EditorGUILayout.BeginVertical("box");
         foreach (var clip in type.clipList)
         {
            EditorGUILayout.BeginHorizontal();
            {
               GUILayout.Space(20);
               EditorGUILayout.BeginHorizontal();
               {
               }
               EditorGUILayout.LabelField(clip.clipName);
               clip.clip = (AudioClip)EditorGUILayout.ObjectField(clip.clip, typeof(AudioClip), true);
               GUILayout.FlexibleSpace();
               GUILayout.Label("#" + i.ToString("00"));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();
            i++;
         }
         EditorGUILayout.EndVertical();
      }


      public void LoadClips(VoiceManager.VoiceType type)
      {
         List<AudioClip> clipList = LoadAllClipWithPath(type.Path);
         foreach (var c in clipList)
         {
            if (type.clipList.Find(cl => cl.clip == c) == null)
               type.clipList.Add(new VoiceManager.VoiceType.ClipInfo()
               {
                  clip = c,
                  clipName = c.name
               }
               );
         }
      }
      List<AudioClip> LoadAllClipWithPath(string path)
      {
         List<AudioClip> returnClips = new List<AudioClip>();
         List<string> aPathFiles = Directory.GetFiles(Application.dataPath.Replace("Assets", "") + path).ToList();
         aPathFiles.RemoveAll(a => a.Contains(".meta"));
         foreach (string file in aPathFiles)
         {
            string assetPath = "Assets" + file.Replace(Application.dataPath, "").Replace('\\', '/');
            AudioClip source = (AudioClip)AssetDatabase.LoadAssetAtPath(assetPath, typeof(AudioClip));
            returnClips.Add(source);
         }
         return returnClips;
      }
   }
}