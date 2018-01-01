using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ItemFlag : ItemBase {

	private const int GetScore = 10000;
	public bool is_Get { get; private set; }

	// Use this for initialization
	void Start() {
		m_ItemVanishTime = 0.2f;
		m_ItemTime = 2.0f;
		m_ItemEfficacy = ItemCategory.Flag;
		is_Get = false;
	}

	// Update is called once per frame
	void Update() {

	}
	/**============================================================
	 * Collision
	 ============================================================*/
	protected new void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			hitItemDirection(other);
			Destroy(transform.root.gameObject, m_ItemVanishTime);
		}
	}
	/**============================================================
	 * Function
	 ============================================================*/
	/**
	 * <summary>
	 * @brief アイテムを取得したときのエフェクト/サウンド
	   </summary>*/
	protected new void hitItemDirection(Collider other) {
		if (other.tag == "Player") {
			PrintScore score;
			score = GameObject.Find("ScoreManager").GetComponent<PrintScore>();
			ExecuteEvents.Execute<ScoreReciever>(
				target: score.gameObject,
				eventData: null,
				functor: (reciever, y) => reciever.ReceivePlayerScore(other.gameObject.GetComponent<PlayerManager>().m_PlayerID, GetScore)
			);
			// ボーナスステージを終了させるフラグを立てる
			is_Get = true;
			// 演出
			AudioList audio;
			audio = GameObject.Find("SoundManager").GetComponent<AudioList>();
			audio.PlayOneShot((int)AudioList.SoundList_SE.SE_ActionPowup);
			EffectControl eff = EffectControl.get();
			eff.createItemHit(other.transform.position);
		}
		// Todo:エフェクトをプレイヤーに追従させる場合はotherにAddすればいい…？
	}

}
