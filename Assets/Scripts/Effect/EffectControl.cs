using UnityEngine;
using System.Collections;

public class EffectControl : MonoBehaviour {

	public GameObject	eff_hit = null;		// ヒットエフェクト.

	// ================================================================ //
	// MonoBehaviour からの継承.

	void 	Start()
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
