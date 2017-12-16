using UnityEngine;

/// <summary>
/// 触れたらゴール
/// </summary>
public class GoalObject : MonoBehaviour {

	public Transform nextStagePoint;		// 次の移動場所
	public float transitionTimer = 2.0f;    // 移行時間
	float timer;
	bool isTimerStart;
	bool isGoal;    // TransitionTimerが終わってから
	int GoalID = 0;	// ゴールプレイヤーのID

	// Use this for initialization
	private void Start() {
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
			DebugPrint.print((transitionTimer - timer).ToString());
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
			GoalID = other.GetComponent<PlayerManager>().getPlayerID();	// ゴールしたプレイヤーのIDを格納
			isTimerStart = true;
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