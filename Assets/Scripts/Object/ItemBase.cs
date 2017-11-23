using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 取得アイテムのベース
/// </summary>
/// 2017年11月23日 oyama add
/// ドキシジェンで書いてみる
public class ItemBase : MonoBehaviour {

	// Todo:まだ想像だけど…
	/**
	 * @enum ItemCategory
	 * アイテムの種類を示す
	 */
	public enum ItemCategory {
		SpeedUp,	
		JumpUp,
		Gravity,
		None,
	};
	protected ItemCategory m_ItemEfficacy;  //! アイテムの効果 
	protected float m_ItemVanishTime;       //! アイテム消滅時間
	protected float m_ItemTime;				//! アイテムの時間
	 
	/**============================================================
	 * override
	 ============================================================*/
	void Start () {
		m_ItemVanishTime = 0.2f;
		m_ItemTime = 2.0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/**============================================================
	 * Collision
	 ============================================================*/
	/// <summary>
	/// 触れたとき
	/// </summary>
	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			hitItemDirection(other);
			Destroy(gameObject, m_ItemVanishTime);
		}
	} 

	/// <summary>
	/// 触れてる間
	/// </summary>
	private void OnTriggerStay(Collider other) {

	}

	private void OnTriggerExit(Collider other) {

	}

	/**============================================================
	 * Function
	 ============================================================*/
	/**
	 * <summary>
	 * @brief アイテムを取得したときのエフェクト/サウンド
	   </summary>*/
	protected void hitItemDirection(Collider other) {
		EffectControl eff = EffectControl.get();
		eff.createItemHit(other.transform.position);

		// Todo:エフェクトをプレイヤーに追従させる場合はotherにAddすればいい…？
	}

	/**============================================================
	 * getter/setter
	 ============================================================*/
	/**
	 * <summary>
	 * @brief アイテムカテゴリーを返す
	   </summary>
	 * @return ItemCategory アイテムの種類
	*/
	public ItemCategory GetItemEfficacy() {
		return m_ItemEfficacy;
	}
}

/**
 * @fn
 * ここに関数の説明を書く
 * @brief 要約説明
 * @param (引数名) 引数の説明
 * @param (引数名) 引数の説明
 * @return 戻り値の説明
 * @sa 参照すべき関数を書けばリンクが貼れる
 * @detail 詳細な説明
 */
