using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpeedUp : ItemBase {


	// Use this for initialization
	void Start () {
		m_ItemVanishTime = 0.2f;
		m_ItemTime = 2.0f;
		m_ItemEfficacy = ItemCategory.SpeedUp;
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(transform.rotation.x, 3f, transform.rotation.z);
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
			ComboSystem combo = other.GetComponent<ComboSystem>();
			for(int i=0; i<3; i++) {
				combo._AddCombo();	// 何回も呼んでスピードを上げる
			}
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
