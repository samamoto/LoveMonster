///**************
///!	@file	:CostomAudioSource.cs
///!	@brief	:
///!	@note	:
///!	@author	:Kashima Yuhei
///**************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

///==============
///!	@class	:CostomAudioSouce
///!	@brief	:1サウンドを管理する
///==============
[System.Serializable]
public class CostomAudioSouce
{
    public AudioClip m_Clip;

    public AudioMixerGroup m_OutputMixer;

    public bool m_PlayOnAwake = false;

    public bool m_Loop = false;

    public bool m_UnableEffect = true;

    [Range(0.0f, 1.0f)]
    public float m_Volume = 1.0f;

    public AudioReverbPreset m_ReverbPreset = AudioReverbPreset.Off;

    private AudioSource m_AudioSource;

    private AudioReverbFilter m_Reverb;

    private bool m_Enable = false;

    ///==============
    ///!	@fn		:Init
    ///!    @brief  :
    ///!	@param	:AudioSource(引数に直接AddComponentする)
    ///!    @param	:AudioReverbFilter(引数に直接AddComponentする)
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Init(AudioSource audioSource, AudioReverbFilter reverb)
    {
        if (!this.m_AudioSource)
            this.m_AudioSource = audioSource;

        if (!this.m_Reverb)
            this.m_Reverb = reverb;

        if (this.m_Clip)
        {
            this.m_AudioSource.clip = this.m_Clip;

            if (this.m_OutputMixer)
                this.m_AudioSource.outputAudioMixerGroup = this.m_OutputMixer;
            else
                Debug.LogWarning("Not resister audiomixergroup");

            this.m_AudioSource.volume = this.m_Volume;
            this.m_AudioSource.playOnAwake = this.m_PlayOnAwake;
            this.m_AudioSource.loop = this.m_Loop;
            this.m_AudioSource.bypassEffects = this.m_UnableEffect;

            this.m_Reverb.reverbPreset = this.m_ReverbPreset;

            this.m_Enable = true;
        }
        else
            Debug.LogError("Not resister audioclip");
    }

    ///==============
    ///!	@fn		:Update
    ///!	@brief	:更新
    ///!	@param	:
    ///!	@retval	:
    ///!	@note	:音量、リバーブエフェクトの適用、リバーブプリセットの変更
    ///==============
    public void Update()
    {
        if (this.m_Enable)
        {
            this.m_AudioSource.volume = this.m_Volume;
            this.m_AudioSource.bypassEffects = this.m_UnableEffect;
            this.m_Reverb.reverbPreset = this.m_ReverbPreset;
        }
        else
            Debug.LogError("Not resister audioclip");
    }

    ///==============
    ///!	@fn		:Play
    ///!	@brief	:再生
    ///!	@param	:
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Play()
    {
        if (this.m_Clip && this.m_Enable)
        {
            this.m_AudioSource.Play();
            Debug.Log("Playing " + this.m_Clip.name);
        }
    }

    ///==============
    ///!	@fn		:PlayOneShot
    ///!	@brief	:ワンショット再生
    ///!	@param	:
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void PlayOneShot()
    {
        if (this.m_Clip && this.m_Enable)
        {
            this.m_AudioSource.PlayOneShot(this.m_Clip, this.m_Volume);
            Debug.Log("Play one shot " + this.m_Clip.name);
        }
    }

    ///==============
    ///!	@fn		:Stop
    ///!	@brief	:停止
    ///!	@param	:
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Stop()
    {
        if (this.m_Clip && this.m_AudioSource.isPlaying && this.m_Enable)
        {
            this.m_AudioSource.Stop();
            Debug.Log("Stop " + this.m_Clip.name);
        }
    }

    ///==============
    ///!	@fn		:Pause
    ///!	@brief	:一時停止
    ///!	@param	:
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Pause()
    {
        if (this.m_Clip && this.m_AudioSource.isPlaying && this.m_Enable)
        {
            this.m_AudioSource.Pause();
            Debug.Log("Pause " + this.m_Clip.name);
        }
    }

    ///==============
    ///!	@fn		:Resume
    ///!	@brief	:一時停止解除
    ///!	@param	:
    ///!	@retval	:
    ///!	@note	:
    ///==============
    public void Resume()
    {
        if (this.m_Clip && this.m_AudioSource.isPlaying && this.m_Enable)
        {
            this.m_AudioSource.UnPause();
            Debug.Log("Resume " + this.m_Clip.name);
        }
    }
}

///***************
///	End of file.
///***************