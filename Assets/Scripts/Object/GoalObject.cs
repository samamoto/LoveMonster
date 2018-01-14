using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 触れたらゴール
/// </summary>
public class GoalObject : MonoBehaviour {

	private const int GetScorePoint = 5000;

	public Transform nextStagePoint;		// 次の移動場所
	private float transitionTimer = 1.0f;    // 移行時間
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
			GoalID = other.GetComponent<PlayerManager>().getPlayerID();	// ゴールしたプレイヤーのIDを格納
			isTimerStart = true;
			// ゴールしたらスコア加算
			PrintScore score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
			ExecuteEvents.Execute<ScoreReciever>(
				target: score.gameObject,
				eventData: null,
				functor: (reciever, y) => reciever.ReceivePlayerScore(other.gameObject.GetComponent<PlayerManager>().getPlayerID(), GetScorePoint)
			);

			// 縮小してテレポートみたいに
			iTween.ScaleTo(other.gameObject, new Vector3(0.00f, 1.6f, 0.00f), 0.75f);
			EffectControl eff = EffectControl.get();
			eff.createBeamUp(other.gameObject, new Vector3(0f, 0f), other.GetComponent<PlayerManager>().getPlayerID());
			eff.createLightJump(other.gameObject, new Vector3(0f, 0f), other.GetComponent<PlayerManager>().getPlayerID());

		}
	}

	private void OnTriggerStay(Collider other) {
		if (other.tag == "Player" && timer > 0f) {
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

	/// <summary>
	/// 次のステージの回転角を返す
	/// </summary>
	/// <returns>回転角度</returns>
	public Quaternion getNextStageRot() {
		return nextStagePoint.rotation;
	}
}