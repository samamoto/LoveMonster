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
public class ObjectActionArea : MonoBehaviour {

	string m_tagValue;
	public GameObject MovePointObject;	// 移動位置に指定するゲームオブジェクト
	Transform m_MoveTrans;
	Vector3 m_MovePos;
	PlayerManager m_PlayerMgr;

	protected void Start() {
		m_tagValue = tag;   // タグの名前コピー
		//m_MoveTrans = GameObject.FindGameObjectWithTag("MovePoint").transform;// Find使いたくない…けど↓で取れない
		//m_MoveTrans = this.GetComponentInChildren<Transform>() as Transform;   // MovePointのTransformを取得
		m_MovePos = MovePointObject.GetComponent<Transform>().position;
		//m_MovePos = m_MoveTrans.TransformPoint(m_MoveTrans.position);	// ワールド座標に変換
	}

	// Update is called once per frame
	void Update () {
		
	}

	// コライダーに当たった
	void OnTriggerEnter(Collider other) {

		// 最初に触れたときだけ初期化する
		if (m_PlayerMgr == null) {
			m_PlayerMgr = other.GetComponent<PlayerManager>();
		}

	}

	// コライダーに当たってる
	void OnTriggerStay(Collider other) {

		// ぶつかった対象にメッセージ(関数)を送る 設定タグがあるか検索
		if (other.tag == "Player" && TagCheck(tag)) {
			m_PlayerMgr.setMovePosition(m_MovePos);//PlayerManagerの移動予定ポイントにMovePointの値をセット
			other.SendMessage(tag, m_tagValue); // 設定されたタグ名(プレイヤーのアニメーション)の関数を呼ぶ
		}
	}

	/// <summary>
	/// 指定したタグがAllPlayerManagerの特別アクションに指定されているかを見る
	/// </summary>
	/// <param name="tag">タグ名</param>
	/// <returns>ある/なし</returns>
	bool TagCheck(string tag) {
		for(int i=0; i<AllPlayerManager.Instance.SpecialAction.Length; i++) {
			if(AllPlayerManager.Instance.SpecialAction[i] == tag) {
				return true;
			}
		}
		return false;
	}

}
