using UnityEngine;
using System.Collections;

public class EffectControl : MonoBehaviour {

	public GameObject	eff_hit = null;     // ヒットエフェクト.
	public GameObject	eff_itemHit = null; // アイテム取得.
	public GameObject eff_speedUp = null;   // スピードアップ.
	public GameObject eff_powUp = null;   // パワーアップ.
	public GameObject eff_dust = null;		// 土埃.

	// ================================================================ //
	// MonoBehaviour からの継承.

	void Start()
	{

	}
	
	void	Update()
	{
	
	}

	// 
	// ビルボードベースのヒットエフェクト
	//
	public void	createHitEffect(Vector3 vec)
	{
		GameObject 	go = Instantiate(this.eff_hit) as GameObject;

		go.AddComponent<Effect>();

		Vector3	position = vec;

		// 位置調整
		position.y += 0.3f;

		go.transform.position = position;
	}

	/// <summary>
	/// アイテム取得の光
	/// </summary>
	public void createItemHit(Vector3 vec) {
		GameObject go = Instantiate(this.eff_itemHit) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		position.y += 0.4f;

		go.transform.position = position;
	}

	/// <summary>
	/// スピードアップ
	/// </summary>
	public void createSpeedUp(Vector3 vec) {
		GameObject go = Instantiate(this.eff_speedUp) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	/// <summary>
	/// パワーアップ
	/// </summary>
	public void createPowerUp(Vector3 vec) {
		GameObject go = Instantiate(this.eff_powUp) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	/// <summary>
	/// 土埃のエフェクト
	/// </summary>
	public void createDust(Vector3 vec) {
		GameObject go = Instantiate(this.eff_dust) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	// ================================================================ //
	//																	//
	// ================================================================ //

	protected static	EffectControl instance = null;

	public static EffectControl	get()
	{
		if(EffectControl.instance == null) {

			GameObject		go = GameObject.Find("EffectControl");

			if(go != null) {

				EffectControl.instance = go.GetComponent<EffectControl>();

			} else {

				Debug.LogError("Can't find game object \"EffectControl\".");
			}
		}

		return(EffectControl.instance);
	}
}
