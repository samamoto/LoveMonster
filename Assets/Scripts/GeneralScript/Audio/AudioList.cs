using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    [SerializeField] public CostomAudioSouce[] m_CostomAudioSouce;

    // Use this for initialization

    private void Awake()
    {
        for (int i = 0; i < this.m_CostomAudioSouce.Length; i++)
        {
            this.m_CostomAudioSouce[i].Init(gameObject.AddComponent<AudioSource>(), gameObject.AddComponent<AudioReverbFilter>());
        }
    }

    private void Update()
    {
        for (int i = 0; i < this.m_CostomAudioSouce.Length; i++)
        {
            this.m_CostomAudioSouce[i].Update();
        }
    }

    public void Play(int play)
    {
        if (0 <= play && play < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[play].Play();
        }
    }

    public void PlayOneShot(int play)
    {
        if (0 <= play && play < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[play].PlayOneShot();
        }
    }

    public void Stop(int stop)
    {
        if (0 <= stop && stop < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[stop].Stop();
        }
    }

    public void Pause(int pause)
    {
        if (0 <= pause && pause < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[pause].Pause();
        }
    }

    public void Resume(int resume)
    {
        if (0 <= resume && resume < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[resume].Resume();
        }
    }
}