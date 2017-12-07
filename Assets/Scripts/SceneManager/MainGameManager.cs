using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(PauseManager))]
[RequireComponent(typeof(TimeManager))]
public class MainGameManager : MonoBehaviour {
    //スクリプト群
    private SceneChange m_ScreenChange;
	private AllPlayerManager m_AllPlayerMgr;
    //private InputManagerGenerator inputmanage;

	public enum PhaseLevel {
		Start,
		CountDown,
		Game,
		Pause,
		Goal,
		None,
	};

	private PhaseLevel m_Phase = PhaseLevel.None;

    // Use this for initialization
    private void Start()
    {
		// シーンが生成されたらStart
		m_Phase = PhaseLevel.Start;
        m_ScreenChange = GetComponent<SceneChange>();
		m_AllPlayerMgr = GameObject.Find("AllPlayerManager").GetComponent<AllPlayerManager>();

    }

    // Update is called once per frame
    private void Update()
    {
		// AllPlayerMgr側でPauseキー押されたことを検知
    }

	/// <summary>
	/// 現在のフェイズを取得する
	/// </summary>
	/// <returns>現在実行中のフェイズ</returns>
	public PhaseLevel getGamePhase() {
		return m_Phase;
	}

	/// <summary>
	/// プレイヤーが死んだ、落ちたときにする処理
	/// AllPlayerManagerから落ちたプレイヤーのIDが投げられる
	/// </summary>
	public void isPlayerDead(int id, Vector3 fallPos) {
		// べつのマネージャと連携するとか
		// 誰かが落ちたら1以上
		// 次回フレームからリスタート場所
		if (id > 0) {

		}
	}

	/// <summary>
	/// ゴールしたプレイヤーに対してする処理
	/// </summary>
	public void isPlayerGoal(int id, Vector3 goalPos) {
		// 誰かがゴールしたら1以上
		if(id > 0) {
			// 演出エフェクトとかUIとか発生させるのに使う
			// 指定時間以上経過したらシーンを遷移させる
			// とりあえず今はさっさと遷移する
			SceneChange.Instance._SceneLoadResult();
		}
	}

	/// <summary>
	/// StartとGoalまでのステージの長さを返す
	/// </summary>
	/// <returns>ステージの直線距離の長さ</returns>
	public float getStageLength() {
		Vector3 start, goal;
		start = GameObject.Find("StartPoint").GetComponent<Transform>().position;
		goal = GameObject.Find("GoalPoint").GetComponent<Transform>().position;
		Debug.Log(Vector3.Distance(start, goal));
		return Vector3.Distance(start, goal);

	}

}
