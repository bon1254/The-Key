using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorTools.GUIStyles;

namespace EditorTools
{
   public class EditorTool : Editor
   {
      #region 取得下拉式選單
      public static bool DrawHeader(string text, InspectorGUIStyle inspstyles)
      {
         string key = text;
         bool state = EditorPrefs.GetBool(key, true);

         GUILayout.BeginHorizontal();
         {
            GUI.changed = false;

            text = "<b><size=11>" + text + "</size></b>";
            if (state) text = "\u25BC " + text;
            else text = "\u25BA " + text;
            if (!GUILayout.Toggle(true, text, ConvertStyletoString(inspstyles))) state = !state;

            if (GUI.changed) EditorPrefs.SetBool(key, state);
         }
         GUILayout.EndHorizontal();
         GUI.backgroundColor = Color.white;
         if (!state) GUILayout.Space(3f);
         return state;
      }
      public static bool DrawHeaderOnOff(string label, InspectorGUIStyle inspstyles, string closelabel = "")
      {
         string key = label;
         bool state = EditorPrefs.GetBool(key, true);

         GUILayout.BeginHorizontal();
         {
            GUI.changed = false;

            label = "<b><size=11>" + label + "</size></b>";
            closelabel = "<b><size=11>" + closelabel + "</size></b>";

            if (state)
               label = "\u25BC " + label;
            else
            {               
               label = "\u25BA " + closelabel;
            }
               

            if (!GUILayout.Toggle(true, label, ConvertStyletoString(inspstyles))) state = !state;

            if (GUI.changed) EditorPrefs.SetBool(key, state);
         }
         GUILayout.EndHorizontal();
         GUI.backgroundColor = Color.white;
         if (!state) GUILayout.Space(3f);
         return state;
      }

      #endregion

      public static void Draw(InspectorGUIStyle inspstyles, params GUILayoutOption[] size)
      {
         EditorGUILayout.BeginHorizontal();
         EditorGUILayout.Toggle(true, ConvertStyletoString(inspstyles), size);
         EditorGUILayout.EndHorizontal();
            //("", true, , size);
      }

      static string ConvertStyletoString(InspectorGUIStyle inspstyles)
      {
         string s = inspstyles.ToString();
         s = s.Replace("_", " ");
         s = s.Replace("DOT", ".");
         return s;
      }
      
      public class Size
      {
         public static GUILayoutOption[] Setsize(float width, float height)
         {
            return new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(height) };
         }
      }
   }

}
