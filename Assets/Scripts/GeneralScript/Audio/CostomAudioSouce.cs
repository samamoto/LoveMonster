using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class CostomAudioSouce
{
    //サウンドクリップ
    public AudioClip m_Clip;

    //アウトプットミキサーへの参照
    public AudioMixerGroup m_OutputMixer;

    public bool m_PlayOnAwake = false;

    //ループフラグ
    public bool m_Loop = false;

    public bool m_UnableEffect = true;

    //音量
    [Range(0.0f, 1.0f)]
    public float m_Volume = 1.0f;

    //リバーブプリセット
    public AudioReverbPreset m_ReverbPreset = AudioReverbPreset.Off;

    public bool m_EnableFadeIn = false;

    //フェードイン時間
    public float m_FaceInTime = 0.5f;

    public bool m_EnableFadeOut = false;

    //フェードアウト時間
    public float m_FadeOutTime = 0.5f;

    private float m_NormalizedTime = 0.0f;

    private enum AudioState
    {
        STATE_NOCLIP,
        STATE_STOP,
        STATE_PLAY,
        STATE_PAUSE,
        STATE_FADEIN,
        STATE_FADEOUT,
        STATE_MAX
    }

    [SerializeField] private AudioState m_State = AudioState.STATE_NOCLIP;
    private AudioState m_TempState = AudioState.STATE_NOCLIP;

    //オーディオソース
    private AudioSource m_AudioSource;

    //リバーブ
    private AudioReverbFilter m_Reverb;

    //使用可能フラグ
    private bool m_Enable = false;

    public void Init(AudioSource audioSource, AudioReverbFilter reverb)
    {
        if (!this.m_AudioSource)
            this.m_AudioSource = audioSource;

        if (!this.m_Reverb)
            this.m_Reverb = reverb;

        if (this.m_Clip)
        {
            this.m_AudioSource.clip = this.m_Clip;
            this.m_State = AudioState.STATE_STOP;

            if (this.m_OutputMixer)
                this.m_AudioSource.outputAudioMixerGroup = this.m_OutputMixer;
            else
                Debug.LogWarning("Not resister audiomixergroup");

            this.m_AudioSource.volume = 0.0f;
            this.m_AudioSource.playOnAwake = this.m_PlayOnAwake;
            this.m_AudioSource.loop = this.m_Loop;
            this.m_AudioSource.bypassEffects = this.m_UnableEffect;

            this.m_Reverb.reverbPreset = this.m_ReverbPreset;

            this.m_Enable = true;
        }
        else
            Debug.LogError("Not resister audioclip");
    }

    public void Update()
    {
        float NowVolume = 0.0f;
        if (this.m_Enable)
        {
            switch (this.m_State)
            {
                case AudioState.STATE_NOCLIP:
                    break;

                case AudioState.STATE_STOP:
                    break;

                case AudioState.STATE_PLAY:

                    //ループしていないサウンドが終了した場合停止状態へ遷移
                    if (!m_AudioSource.isPlaying)
                        this.m_State = AudioState.STATE_STOP;
                    else
                        this.m_AudioSource.volume = this.m_Volume;
                    break;

                case AudioState.STATE_PAUSE:
                    break;

                case AudioState.STATE_FADEIN:
                    //フェードイン処理
                    this.m_NormalizedTime += Mathf.Clamp01(Time.deltaTime / this.m_FaceInTime);
                    NowVolume = Mathf.Lerp(0.0f, this.m_Volume, this.m_NormalizedTime);
                    if (NowVolume >= m_Volume)
                    {
                        this.m_AudioSource.volume = this.m_Volume;
                        this.m_State = AudioState.STATE_PLAY;
                    }
                    else
                    {
                        this.m_AudioSource.volume = NowVolume;
                    }
                    break;

                case AudioState.STATE_FADEOUT:
                    //フェードアウト処理
                    this.m_NormalizedTime += Mathf.Clamp01(Time.deltaTime / this.m_FadeOutTime);
                    NowVolume = Mathf.Lerp(this.m_Volume, 0, this.m_NormalizedTime);
                    if (NowVolume <= 0.0f)
                    {
                        this.m_AudioSource.Stop();
                        this.m_State = AudioState.STATE_STOP;
                    }
                    else
                    {
                        this.m_AudioSource.volume = NowVolume;
                    }
                    break;

                case AudioState.STATE_MAX:
                    break;
            }

            this.m_AudioSource.bypassEffects = this.m_UnableEffect;
            this.m_Reverb.reverbPreset = this.m_ReverbPreset;
        }
        else
            Debug.LogError("Not resister audioclip");
    }

    public void Play()
    {
        if (this.m_Clip && this.m_Enable)
        {
            this.m_AudioSource.Play();
            this.m_NormalizedTime = 0.0f;

            //フェードイン
            if (this.m_EnableFadeIn)
                this.m_State = AudioState.STATE_FADEIN;
            else
                this.m_State = AudioState.STATE_PLAY;
            //Debug.Log("Playing " + this.m_Clip.name);
        }
    }

    public void PlayOneShot()
    {
        if (this.m_Clip && this.m_Enable)
        {
            this.m_AudioSource.PlayOneShot(this.m_Clip, this.m_Volume);
            this.m_State = AudioState.STATE_PLAY;
            //Debug.Log("Play one shot " + this.m_Clip.name);
        }
    }

    public void Stop()
    {
        if (this.m_Clip && this.m_AudioSource.isPlaying && this.m_Enable)
        {
            this.m_NormalizedTime = 0.0f;

            //フェードアウト
            if (this.m_EnableFadeOut)
            {
                this.m_State = AudioState.STATE_FADEOUT;
            }
            else
            {
                this.m_State = AudioState.STATE_STOP;
                this.m_AudioSource.Stop();
            }
            //Debug.Log("Stop " + this.m_Clip.name);
        }
    }

    public void Pause()
    {
        if (this.m_Clip && this.m_AudioSource.isPlaying && this.m_Enable)
        {
            this.m_AudioSource.Pause();
            this.m_TempState = this.m_State;
            this.m_State = AudioState.STATE_PAUSE;
            //Debug.Log("Pause " + this.m_Clip.name);
        }
    }

    public void Resume()
    {
        if (this.m_Clip && this.m_AudioSource.isPlaying && this.m_Enable)
        {
            this.m_AudioSource.UnPause();
            this.m_State = this.m_TempState;
            //Debug.Log("Resume " + this.m_Clip.name);
        }
    }
}