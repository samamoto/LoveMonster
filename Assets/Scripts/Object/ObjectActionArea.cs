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
/// Todo:複数プレイヤー対応　2017/11/19
/// 

[SelectionBase]//Sceneビューでキャラクタを選択したときにこのスクリプトの入っているヒエラルキーが優先して選択されるようになる
public class ObjectActionArea : MonoBehaviour {

	public GameObject MovePointObject;  // 移動位置に指定するゲームオブジェクト
	[SerializeField] List<Vector3> m_MovePos = new List<Vector3>();

	public Dictionary<int ,MoveState> m_MoveTask = new Dictionary<int, MoveState>();
	public List<int> m_MoveID = new List<int>();

	MoveState m_MoveState;
	bool is_FoundActTag;

	public int MaxActionNum = 4;  // 何人まで同時実行か
	int[] actID = new int[ConstPlayerParameter.PlayerMax];		// Playerの実行順


	protected void Start() {
		Transform[] trs;
		trs = MovePointObject.GetComponentsInChildren<Transform>();   // くっついてるポジションを全部
																	  // 変換
		for (int i = 0; i < trs.Length; i++) {
			m_MovePos.Add(trs[i].position);
		}
		//m_MovePos = m_MoveTrans.TransformPoint(m_MoveTrans.position);	// ワールド座標に変換
		is_FoundActTag = AllPlayerManager.TagCheck(tag);                // 設定されたタグがあるかどうか
	}

	// コライダーに当たった
	void OnTriggerEnter(Collider other) {

#if DEBUG
		// 見つからなかったらLog
		if (!is_FoundActTag)
			Debug.Log(name + ":" + tag + " NotFound! 設定されたアクションはリストにありません");
#endif



		// 以前に触れたプレイヤーなら消すか追加しない
		//if(other.GetComponent<PlayerManager>())
		//m_MoveTask.Add(other.GetComponent<MoveState>());


		/*
		for (int i = 0; i < MaxActionNum; i++) {
			if (m_MoveState[i] == null) {
				m_MoveState[i] = other.GetComponent<MoveState>();
			}

			// PlayerIDとiが一致したら処理
			if(i == m_MoveState[i].getPlayerID()) {
				m_MoveState[i].setMovePosition(m_MovePos.ToArray());//PlayerManagerの移動予定ポイントにMovePointの値をセット			
				actID[i] = m_MoveState[i].getPlayerID();  // アクションをしようとしているPlayerのID
			}
		}
		*/
		// 新しく触れた物だけ状態を更新して上書きする
		// ObjectManagerに送る
		// 現在アクションが実行中かどうかは判定してない
	}

	// コライダーに当たってる
	void OnTriggerStay(Collider other) {

		int id = other.GetComponent<PlayerManager>().getPlayerID();
		// MoveIDに登録
		// 重複してない？
		bool found = false;
		for (int i = 0; i < m_MoveID.Count; i++) {
			if (m_MoveID[i] == id) {
				found = true;
				break;
			}
		}

		if (!found) { 
			// いなければ追加
			m_MoveID.Add(id);
			// タスクに登録
			m_MoveTask.Add(id, other.GetComponent<MoveState>());
			// セット
			m_MoveTask[id].setMovePosition(m_MovePos.ToArray());//PlayerManagerの移動予定ポイントにMovePointの値をセット
		}

		// まだ実行されてない
		if(m_MoveTask[id].isMove() == false){
			// ぶつかった対象にメッセージ(関数)を送る 設定タグがあるか検索
			if (other.tag == "Player" && is_FoundActTag) {
				other.SendMessage("PlayAction", tag);   // 設定されたタグ名を渡してアクションを再生する関数を呼ぶ　依存性を弱めたいからSend
			}
		} else {
			// 実行されてたら消す
			m_MoveTask.Remove(id);
			m_MoveID.Remove(id);
		}
	}

	private void OnTriggerExit(Collider other) {
		
		int id = other.GetComponent<PlayerManager>().getPlayerID();
		MoveState tmp;
		// 抜けても消す
		if (m_MoveTask.TryGetValue(id, out tmp)){	// 初期値なら消えてる
			if(tmp == null) {
				m_MoveTask.Remove(id);
				m_MoveID.Remove(id);
			}
		}
	}
}
