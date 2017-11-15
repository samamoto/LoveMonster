using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//============================================================
// ==2017/10/31 Oyama Add
//============================================================
// オブジェクトの前に置いて衝突したプレイヤーの関数を呼ぶ
// おもにブロックごとの指定アクションを再生させるための機構
// 親子関係を取るのでMovePointに移動予定のオブジェクトをくっつけて管理する

/// Todo:タグを使っているのでどっかでまとめて管理しているところと連携した方が呼び出しミスとかなくなって良いか
/// 
[SelectionBase]//Sceneビューでキャラクタを選択したときにこのスクリプトの入っているヒエラルキーが優先して選択されるようになる
public class ObjectActionArea : MonoBehaviour {

	public GameObject MovePointObject;  // 移動位置に指定するゲームオブジェクト
	[SerializeField] List<Vector3> m_MovePos = new List<Vector3>();

	MoveState m_MoveState;
	bool is_FoundActTag;

	protected void Start() {
		Transform[] trs;
		//m_MoveTrans = GameObject.FindGameObjectWithTag("MovePoint").transform;// Find使いたくない…けど↓で取れない
		//m_MoveTrans = this.GetComponentInChildren<Transform>() as Transform;   // MovePointのTransformを取得
		trs = MovePointObject.GetComponentsInChildren<Transform>();   // くっついてるポジションを全部
		// 変換
		for (int i = 0; i < trs.Length; i++) {
			m_MovePos.Add(trs[i].position);
		}
		//m_MovePos = m_MoveTrans.TransformPoint(m_MoveTrans.position);	// ワールド座標に変換
		is_FoundActTag = AllPlayerManager.TagCheck(tag);				// 設定されたタグがあるかどうか
	}

	// コライダーに当たった
	void OnTriggerEnter(Collider other) {

#if DEBUG
		// 見つからなかったらLog
		if (!is_FoundActTag)
			Debug.Log(name + ":" + tag + " NotFound! 設定されたアクションはリストにありません");
#endif

		// 最初に触れたときだけ初期化する
		if (m_MoveState == null) {
			m_MoveState = other.GetComponent<MoveState>();
		}
		m_MoveState.setMovePosition(m_MovePos.ToArray());//PlayerManagerの移動予定ポイントにMovePointの値をセット			

	}

	// コライダーに当たってる
	void OnTriggerStay(Collider other) {

		// ぶつかった対象にメッセージ(関数)を送る 設定タグがあるか検索
		if (other.tag == "Player" && is_FoundActTag) {
			// 配列対応
			other.SendMessage("PlayAction", tag);	// 設定されたタグ名を渡してアクションを再生する関数を呼ぶ　依存性を弱めたいからSend
			//m_PlayerMgr.PlayAction(tag);			// これでもできるけど
		}
	}

}
