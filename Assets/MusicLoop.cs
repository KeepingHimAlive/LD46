using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MusicLoop : MonoBehaviour
{
    private AudioSource[] _sources;
    
    public AudioClip[] _clips;
    private double[] clipLengths;

    public int toggle = 0;

    public double nextStartTime;
    
    private void Start()
    {
        
        clipLengths = new double[_clips.Length]; 
        for (var i = 0; i < _clips.Length; i++)
        {
            var clip = _clips[i];
            clipLengths[i] = (double) clip.samples / clip.frequency;
        }

        _sources = GetComponents<AudioSource>();
        
        var randomClipIndex = Random.Range(0, 2);
        _sources[toggle].clip = _clips[randomClipIndex];
        _sources[toggle].PlayScheduled(AudioSettings.dspTime + .2);
        nextStartTime = AudioSettings.dspTime + .2 + clipLengths[randomClipIndex];

        toggle = 1 - toggle;
    }

    private void Update()
    {
        if (AudioSettings.dspTime > nextStartTime - 1)
        {
            int randomClipIndex = Random.Range(0, 2);
            AudioClip nextClip = _clips[randomClipIndex];
            double nextClipDuration = (double) nextClip.samples / nextClip.frequency;
            
            _sources[toggle].clip = nextClip;
            _sources[toggle].PlayScheduled(nextStartTime);
            
            toggle = 1 - toggle;
            nextStartTime += nextClipDuration;
        }
    }
}
