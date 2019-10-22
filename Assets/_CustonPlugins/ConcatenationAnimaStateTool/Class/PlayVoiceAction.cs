using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Funcy.ConcatenationAnimaStateTool
{
   public class PlayVoiceAction : MonoBehaviour
   {
      public static void PlaySound_Static(int typeID, string SoundManagerClipName)
      {
         VoiceManager.GetUnUsingSources.PlayOneShot(VoiceManager.GetClipByName(typeID, SoundManagerClipName));
      }
      public void PlaySound_Public(int typeID, string SoundManagerClipName)
      {
         VoiceManager.GetUnUsingSources.PlayOneShot(VoiceManager.GetClipByName(typeID, SoundManagerClipName));
      }
      public void PlaySound_Name(string SoundManagerClipName)
      {
         VoiceManager.GetUnUsingSources.PlayOneShot(VoiceManager.GetClipByName(0, SoundManagerClipName));
      }
   }
}