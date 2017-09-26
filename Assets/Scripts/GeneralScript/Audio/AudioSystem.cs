using UnityEngine;

public class AudioSystem : Singleton<AudioSystem>
{
    //BGM用AudioSource
    private AudioSource m_BGM;

    //SE用AudioSource
    private AudioSource m_SE;

    public void _PlayBGM(AudioClip playAudio, float vol, bool loop)
    {
        if (m_BGM == null)
        {
            m_BGM = this.gameObject.AddComponent<AudioSource>();
            m_BGM.playOnAwake = false;
        }
        if (!m_BGM.isPlaying)
        {
            //サウンドクリップ登録
            m_BGM.clip = playAudio;
            m_BGM.volume = vol;
            //ループフラグ
            m_BGM.loop = loop;
            //再生(重複不可能)
            m_BGM.Play();
            Debug.Log("Play:BGM -> " + m_BGM.clip.name);
        }
        else
        {
            Debug.LogWarning("再生中です。 -> " + m_BGM.clip.name);
        }
    }

    public void _PauseBGM()
    {
        if (m_BGM.clip != null)
        {
            if (m_BGM.isPlaying)
            {
                //ポーズ
                m_BGM.Pause();
                Debug.Log("Paum_SE:BGM -> " + m_BGM.clip.name);
            }
            else
            {
                //ポーズ解除
                m_BGM.UnPause();
                Debug.Log("Resume:BGM -> " + m_BGM.clip.name);
            }
        }
        else
        {
            Debug.LogWarning("クリップが登録させていません。");
        }
    }

    public void _StopBGM()
    {
        if (m_BGM.clip != null)
            if (m_BGM.isPlaying)
            {
                //停止
                m_BGM.Stop();
                //サウンドクリップ登録解除
                m_BGM.clip = null;
                Debug.Log("Stop:BGM");
            }
    }

    public void _PlaySE(AudioClip playAudio, float vol)
    {
        if (m_SE == null)
        {
            m_SE = this.gameObject.AddComponent<AudioSource>();
            m_SE.playOnAwake = false;
        }
        m_SE.volume = vol;
        //ワンショット再生(重複可能)
        m_SE.PlayOneShot(playAudio);
        Debug.Log("Play:SE -> " + playAudio.name);
        return;
    }
}