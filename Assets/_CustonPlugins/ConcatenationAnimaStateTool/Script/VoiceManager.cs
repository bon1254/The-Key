using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Funcy.ConcatenationAnimaStateTool
{
   [ExecuteInEditMode]
   public class VoiceManager : MonoBehaviour
   {
      public static VoiceManager This;
      [Range(0, 10)] public int requireSourecsCount = 5;
      public List<AudioSource> SourceList = new List<AudioSource>();

      public List<VoiceType> voiceType = new List<VoiceType>();
      [Serializable] public class VoiceType
      {
         public string Path = "";
         public List<ClipInfo> clipList = new List<ClipInfo>();
         [Serializable]
         public class ClipInfo
         {
            public string clipName = "";
            public AudioClip clip;
         }
      }

      void Update()
      {
         if (!This) This = this;

         if (Application.isPlaying) return;

         CheckSources();
      }
      private void CheckSources()
      {
         requireSourecsCount = requireSourecsCount < 0 ? 0 : requireSourecsCount > 10 ? 10 : requireSourecsCount;
         if (SourceList.Count < requireSourecsCount)
         {
            for (var i = 0; i < requireSourecsCount; i++)
            {
               AudioSource A = new GameObject("Sources", typeof(AudioSource)).GetComponent<AudioSource>();
               A.transform.parent = transform;
               SourceList.Add(A);
            }


         }
         else if (SourceList.Count > requireSourecsCount)
         {
            for (var i = SourceList.Count - 1; i >= requireSourecsCount; i--)
            {
               DestroyImmediate(SourceList[i].gameObject);
               SourceList.Remove(SourceList[i]);
            }

         }
      }


      public static AudioClip GetClipByName(int VoiceTypeid, string name)
      {
         return This.voiceType[VoiceTypeid].clipList.Find(c => c.clipName == name).clip;
      }
      public static AudioClip GetClipByIndex(int VoiceTypeid, int id)
      {
         return This.voiceType[VoiceTypeid].clipList[id].clip;
      }
      public static AudioSource GetUnUsingSources
      {
         get { return This.SourceList.Find(s => !s.isPlaying); }
         set { }
      }

      #region PlayMaker
      public int GetTypeCount(int VoiceTypeid)
      {
         return This.voiceType[VoiceTypeid].clipList.Count;
      }
      public bool VoiceManagerPlaying()
      {
         return GetComponent<AudioSource>().isPlaying;
      }
      public void PlayVoice(int typeID, int clipID)
      {
         GetComponent<AudioSource>().PlayOneShot(GetClipByIndex(typeID, clipID));
      }
      #endregion
   }
}