using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.Threading.Tasks;
public class StatePlayVoice : StateMachineManager
{
    [Serializable]
    public class VoiceInfo
    {
        public int selectType = 0;
        public int index = 0;
        public bool loop = false; public bool fadeIn = false; public bool fadeOut = false;

        public bool randomPich = false; public Vector2 PichRandomRange = Vector2.one;

        public float fadeSpeedIn = 1.0f; public float fadeSpeedOut = 1.0f;
        public AudioClip clip;
        public float playTimelate = 0.0f;
    }
    public List<VoiceInfo> voiceInfoList = new List<VoiceInfo>();
    bool onState = false;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onState = true;
        var source = animator.GetComponentsInChildren<AudioSource>().ToList().Find(a => !a.isPlaying);
        if (source == null)
        {
            source = animator.GetComponentInChildren<AudioSource>();
            source = animator.gameObject.AddComponent<AudioSource>();
            CopyComponent(animator.GetComponent<AudioSource>(), source);
        }
        source.spatialize = true;
        playing(source, animator);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onState = false;
    }
    void playing(AudioSource source, Animator animator)
    {
        foreach (var info in voiceInfoList)
            play(info, source, animator);
    }
    async void play(VoiceInfo info, AudioSource source, Animator animator)
    {
        if (info.playTimelate > 0)
            await wait(info.playTimelate);

        source.loop = info.loop;
        source.clip = info.clip;
        source.Play();

        if (info.loop && info.fadeIn)
        {
            source.volume = 0;
            await FadeSourcesVolume(source, 1);
        }


        while (onState) await Task.Delay(1000 / 60);

        if (info.loop && info.fadeOut)
        {
            source.volume = 1;
            await FadeSourcesVolume(source, 0);
        }


        if (onState || info.loop)
        {
            foreach (var s in animator.GetComponentsInChildren<AudioSource>())
                s.Stop();
        }

        source.volume = 1;
    }
    async Task FadeSourcesVolume(AudioSource source, float targetValue)
    {
        while (Mathf.Abs(targetValue - source.volume) > 0.01f)
        {
            await Task.Delay(1000 / 60);
            source.volume = Mathf.Lerp(source.volume, targetValue, 0.1f);
        }
    }
    async Task wait(float time)
    {
        float now = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - now < time) await Task.Delay(1000 / 60);
    }

    Component CopyComponent(Component original, Component destination)
    {
        Type type = original.GetType();
        Component copy = destination;
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}

