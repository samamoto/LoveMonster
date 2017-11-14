using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//============================================================
// ==2017/10/31 Oyama Add
//============================================================
// オブジェクトの前に置いて衝突したプレイヤーの関数を呼ぶ
// おもにブロックごとの指定アクションを再生させるための機構
//
/// Todo:タグを使っているのでどっかでまとめて管理しているところと連携した方が呼び出しミスとかなくなって良いか
/// 
[SelectionBase]//Sceneビューでキャラクタを選択したときにこのスクリプトの入っているヒエラルキーが優先して選択されるようになる
public class ObjectActionArea : MonoBehaviour {

	public GameObject MovePointObject;	// 移動位置に指定するゲームオブジェクト
	Vector3 m_MovePos;
	PlayerManager m_PlayerMgr;
	bool is_FoundActTag;

	protected void Start() {
		//m_MoveTrans = GameObject.FindGameObjectWithTag("MovePoint").transform;// Find使いたくない…けど↓で取れない
		//m_MoveTrans = this.GetComponentInChildren<Transform>() as Transform;   // MovePointのTransformを取得
		m_MovePos = MovePointObject.GetComponent<Transform>().position;
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
			if (m_PlayerMgr == null) {
			m_PlayerMgr = other.GetComponent<PlayerManager>();
		}

	}

	// コライダーに当たってる
	void OnTriggerStay(Collider other) {

		// ぶつかった対象にメッセージ(関数)を送る 設定タグがあるか検索
		if (other.tag == "Player" && is_FoundActTag) {
			m_PlayerMgr.setMovePosition(m_MovePos);//PlayerManagerの移動予定ポイントにMovePointの値をセット
			other.SendMessage("PlayAction", tag);	// 設定されたタグ名を渡してアクションを再生する関数を呼ぶ　依存性を弱めたいからSend
			//m_PlayerMgr.PlayAction(tag);			// これでもできるけど
		}
	}

}
