using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioList : MonoBehaviour
{
    [SerializeField] public CostomAudioSouce[] m_CostomAudioSouce;
	// oyama
	public enum SoundList_BGM {
		BGM_Game_Stage0 = 0,
		BGM_Game_Stage1,
		BGM_Game_Stage2,
		BGM_Game_Stage3,
		BGM_Game_Bonus0,
		BGM_Tutorial0,
		BGM_None,
	}
	public enum SoundList_SE {
		SE_ActionComboBad = SoundList_BGM.BGM_None,	// 繋がってる
		SE_ActionComboGood,
		SE_ActionGrab,
		SE_ActionHit,
		SE_ActionJump,
		SE_ActionLanding,
		SE_ActionSlide,
		SE_ActionVault,
		SE_ActionCountDown,
		SE_ActionPowup,
		SE_SceneMove,
		SE_UI_Menu_Decision,
		SE_UI_Menu_Cancel,
		SE_UI_Menu_Cursor,
		SE_UI_Title_Decision,
        SE_ActionRolling,
        SE_ActionHandrail,
        SE_Clear,
        SE_StageWarp,
        SE_Bonus,
		SE_TensionCharge,
        SE_Kick,
        SE_None,
	}



	private void Awake()
    {
		AudioReverbFilter filter = gameObject.AddComponent<AudioReverbFilter>();
        for (int i = 0; i < this.m_CostomAudioSouce.Length; i++)
        {
            this.m_CostomAudioSouce[i].Init(gameObject.AddComponent<AudioSource>(),filter );
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