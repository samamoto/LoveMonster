using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 触れたらゴール
/// </summary>
public class GoalObject : MonoBehaviour {

    private AudioList m_Audio;      // 音声再生用
    private const int GetScorePoint = 5000;

	public Transform nextStagePoint;		// 次の移動場所
	public float transitionTimer = 2.0f;    // 移行時間
	float timer;
	bool isTimerStart;
	bool isGoal;    // TransitionTimerが終わってから
	int GoalID = 0;	// ゴールプレイヤーのID

	// Use this for initialization
	private void Start() {
        m_Audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
    }

    // Update is called once per frame
    private void Update() {
		// ゴールしていたらリセットする
		if (isGoal) {
			resetGoal();
		}

		if (isTimerStart) {
			timer += Time.deltaTime;
#if DEBUG
			//DebugPrint.print((transitionTimer - timer).ToString());
#endif
			if(timer >= transitionTimer) {
				isGoal = true;

				//SceneChange.Instance._SceneLoadResult();
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			EffectControl eff = EffectControl.get();
			eff.createClearShine(other.transform.position);

            m_Audio.PlayOneShot((int)AudioList.SoundList_SE.SE_Clear);

            GoalID = other.GetComponent<PlayerManager>().getPlayerID();	// ゴールしたプレイヤーのIDを格納
			isTimerStart = true;
			// ゴールしたらスコア加算
			PrintScore score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
			ExecuteEvents.Execute<ScoreReciever>(
				target: score.gameObject,
				eventData: null,
				functor: (reciever, y) => reciever.ReceivePlayerScore(other.gameObject.GetComponent<PlayerManager>().getPlayerID(), GetScorePoint)
			);
		}
	}

	private void resetGoal() {
		isTimerStart = isGoal = false;
		timer = 0f;
		GoalID = 0;
	}

	/// <summary>
	/// 誰かがゴールしたらtrue
	/// </summary>
	public bool getGoal() {
		return isGoal;
	}

	/// <summary>
	/// ゴールしたプレイヤーのIDを返す
	/// ゴールしていない場合は0
	/// </summary>
	public int getGoalPlayerNo() {
		return GoalID;
	}

	/// <summary>
	/// 次のステージのスタートポイントを示す
	/// </summary>
	public Vector3 getNextStagePoint() {
		return nextStagePoint.position;
	}
}