using System.Collections.Generic;
using System;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public static event Action OnStep;


    [Header("Step")]
    [SerializeField] private AudioSource stepSource;
    [SerializeField] private List<AudioClip> stepClips;





    public void Step()
    {
        Debug.Log("Play step sound!");
        OnStep?.Invoke();
        PlayClip(stepClips, stepSource);
    }


    private int clipIndex;
    private void PlayClip(List<AudioClip> clips, AudioSource source)
    {
        if (clips.Count == 0) return;

        clipIndex = UnityEngine.Random.Range(0, clips.Count);

        source.PlayOneShot(clips[clipIndex]);
    }

}
