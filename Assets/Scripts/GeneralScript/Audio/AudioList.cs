///**************
///!	@file	:AudioList.cs
///!	@brief	:
///!	@note	:
///!	@author	:Kashima Yuhei
///**************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///==============
///!	@class	:AudioList
///!	@brief	:オーディオリスト
///==============
public class AudioList : MonoBehaviour
{
    [SerializeField] public CostomAudioSouce[] m_CostomAudioSouce;

    /// Use this for initialization

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

    ///==============
    ///!	@fn	    :Play
    ///!	@brief	:再生
    ///!	@param	:再生させるサウンド番号
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Play(int play)
    {
        if (0 <= play && play < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[play].Play();
        }
    }

    ///==============
    ///!	@fn	    :PlayOneShot
    ///!	@brief	:ワンショット再生
    ///!	@param	:再生させるサウンド番号
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void PlayOneShot(int play)
    {
        if (0 <= play && play < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[play].PlayOneShot();
        }
    }

    ///==============
    ///!	@fn		:Stop
    ///!	@brief	:停止
    ///!	@param	:停止させるサウンド番号
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Stop(int stop)
    {
        if (0 <= stop && stop < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[stop].Stop();
        }
    }

    ///==============
    ///!	@fn		:Pause
    ///!	@brief	:一時停止
    ///!	@param	:一時停止させるサウンド番号
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Pause(int pause)
    {
        if (0 <= pause && pause < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[pause].Pause();
        }
    }

    ///==============
    ///!	@fn		:Resume
    ///!	@brief	:一時停止解除
    ///!	@param	:一時停止を解除するサウンド番号
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Resume(int resume)
    {
        if (0 <= resume && resume < this.m_CostomAudioSouce.Length)
        {
            this.m_CostomAudioSouce[resume].Resume();
        }
    }
}

///***************
///	End of file.
///***************