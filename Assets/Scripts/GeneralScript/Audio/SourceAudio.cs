using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CustomAudioClip
{
    //オーディオクリップ
    public AudioClip Clip;

    //ボリューム(0.0f~1.0f)
    public float Vol;

    public void Load(string dir, float vol)
    {
        this.Clip = Resources.Load(dir, typeof(AudioClip)) as AudioClip;
        this.Vol = vol;
    }
}

public class SourceAudio : MonoBehaviour
{
    //サウンドデータを登録
    public CustomAudioClip[] m_Audio;

    //再生には配列番号が引数にあるのでenum等で番号を控えておくと良い。

    //BGM再生(音の重複は不可能)
    //(使用サウンド配列番号,ループフラグ)
    public void _PlayBGM(int playNo, bool loop)
    {
        if (this._CheckExist(playNo))
        {
            AudioSystem.Instance._PlayBGM(m_Audio[playNo].Clip, m_Audio[playNo].Vol, loop);
        }
    }

    //BGM一時停止&再開
    public void _PauseBGM()
    {
        AudioSystem.Instance._PauseBGM();
    }

    //BGM停止
    public void _StopBGM()
    {
        AudioSystem.Instance._StopBGM();
    }

    //SE再生(重複可能)
    //(使用サウンド配列番号)
    public void PlaySE(int playNo)
    {
        if (this._CheckExist(playNo))
        {
            AudioSystem.Instance._PlaySE(m_Audio[playNo].Clip, m_Audio[playNo].Vol);
        }
    }

    private bool _CheckExist(int No)
    {
        if (m_Audio == null)
        {
            Debug.LogWarning("サウンドのインスタンスが１つもありません。");
            return false;
        }
        if (No < m_Audio.Length)
        {
            if (m_Audio[No].Clip == null)
            {
                Debug.LogWarning("クリップにサウンドがありません");
                return false;
            }
        }
        else
        {
            Debug.LogWarning("登録されていない音源です。");
            return false;
        }
        return true;
    }
}