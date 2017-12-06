using UnityEngine;

/// <summary>
/// 触れたらゴール
/// </summary>
public class GoalObject : MonoBehaviour {

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
		if (isTimerStart) {
			timer += Time.deltaTime;
#if DEBUG
			DebugPrint.print((transitionTimer - timer).ToString());
#endif
			if(timer >= transitionTimer) {
				isGoal = true;
				SceneChange.Instance._SceneLoadResult();
			}
		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.tag == "Player") {
			GoalID = other.GetComponent<PlayerManager>().getPlayerID();	// ゴールしたプレイヤーのIDを格納
			isTimerStart = true;
		}
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

}