using UnityEngine;

public class EffectControl : MonoBehaviour {
	public GameObject eff_hit = null;     // ヒットエフェクト.
	public GameObject eff_itemHit = null; // アイテム取得.
	public GameObject eff_speedUp = null;   // スピードアップ.
	public GameObject eff_powUp = null;   // パワーアップ.
	public GameObject eff_dust = null;      // 土埃.
	public GameObject eff_burst = null;     // 爆発
	public GameObject eff_clearShine = null;      // クリア時の光
	public GameObject eff_actionTrail = null;      // アクションの軌跡
	public GameObject eff_lightPulse = null;      // 光のパルス
	public GameObject eff_lightTrail = null;        // 光の軌跡
	public GameObject eff_slideSpark = null;        // スライド時の火花
	public GameObject[] eff_beamUp = null;            // 転送時
	public GameObject[] eff_LightJump = null;         // 
	public GameObject[] eff_Buff = null;            // 能力上昇
	public GameObject[] eff_chargeAura = null;      // チャージ
	public GameObject[] eff_PlasmaImpact = null;    // プラズマ
	public GameObject[] eff_Nova = null;            // ノヴァ
	public GameObject[] eff_MuzzleFlash = null;		// マズルフラッシュ
	// ================================================================ //

	public void createMuzzleFlash(Vector3 vec, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_MuzzleFlash[c - 1]) as GameObject;
		go.AddComponent<Effect>();
		Vector3 position = vec;
		// 位置調整
		position.y += 0.0f;
		go.transform.position = position;
	}


	public void createNova(Vector3 vec, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_Nova[c - 1]) as GameObject;
		go.AddComponent<Effect>();
		Vector3 position = vec;
		// 位置調整
		position.y += 0.0f;
		go.transform.position = position;
	}



	public void createPlasmaImpact(Vector3 vec, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_PlasmaImpact[c - 1]) as GameObject;
		go.AddComponent<Effect>();
		Vector3 position = vec;
		// 位置調整
		position.y += 0.0f;
		go.transform.position = position;
	}

	public void createCharge(Vector3 vec, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_chargeAura[c - 1]) as GameObject;
		go.AddComponent<Effect>();
		Vector3 position = vec;
		// 位置調整
		position.y += 0.9f;
		go.transform.position = position;
	}

	/// <summary>
	/// 能力上昇
	/// </summary>
	/// <param name="vec">位置</param>
	/// <param name="color">1P~4P</param>
	public void createBuff(Vector3 vec, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_Buff[c-1]) as GameObject;
		go.AddComponent<Effect>();
		Vector3 position = vec;
		// 位置調整
		position.y += 0.0f;
		go.transform.position = position;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="vec"></param>
	public void createLightJump(Vector3 vec, Vector3 rot, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_LightJump[c - 1]) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		position.y += 0.9f;

		go.transform.position = position;
		go.transform.eulerAngles = new Vector3(90f, 0,0)+rot;
	}

	/// <summary>
	/// 上にビームが上昇する感じのエフェクト
	/// </summary>
	/// <param name="vec"></param>
	public void createBeamUp(Vector3 vec, int color) {
		int c = color;
		if (c > eff_Buff.Length || c == 0) c = 1;

		GameObject go = Instantiate(eff_beamUp[c - 1]) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		position.y += 0.9f;

		go.transform.position = position;
	}

	//
	// ビルボードベースのヒットエフェクト
	//
	public void createHitEffect(Vector3 vec) {
		GameObject go = Instantiate(this.eff_hit) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

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

	/// <summary>
	/// 爆発エフェクト
	/// </summary>
	public void createBurst(Vector3 vec) {
		GameObject go = Instantiate(this.eff_burst) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	/// <summary>
	/// 光の軌跡
	/// </summary>
	public void createLightTrail(Vector3 vec) {
		GameObject go = Instantiate(this.eff_lightTrail) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	/// <summary>
	/// 光のパルス
	/// </summary>
	public void createLightPulse(Vector3 vec) {
		GameObject go = Instantiate(this.eff_lightPulse) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	/// <summary>
	/// クリア時の光
	/// </summary>
	public void createClearShine(Vector3 vec) {
		GameObject go = Instantiate(this.eff_clearShine) as GameObject;

		go.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		//position.y += 0.1f;

		go.transform.position = position;
	}

	/// <summary>
	/// アクションの軌跡
	/// </summary>
	public void createActionTrail(Vector3 vec, GameObject obj) {
		obj = Instantiate(this.eff_actionTrail) as GameObject;

		obj.AddComponent<Effect>();

		Vector3 position = vec;

		// 位置調整
		position.y += 0.9f;

		obj.transform.position = position;
	}

	/// <summary>
	/// スライド時の火花
	/// </summary>
	public void createSlideSpark(Vector3 vec, GameObject obj) {
		obj = Instantiate(this.eff_slideSpark) as GameObject;

		obj.AddComponent<EffectTrailManager>();

		Vector3 position = vec;

		// 位置調整
		position.y -= 0.2f;

		obj.transform.position = position;
	}

	// ================================================================ //
	//																	//
	// ================================================================ //

	protected static EffectControl instance = null;

	public static EffectControl get() {
		if (EffectControl.instance == null) {
			GameObject go = GameObject.Find("EffectControl");

			if (go != null) {
				EffectControl.instance = go.GetComponent<EffectControl>();
			} else {
				Debug.LogError("Can't find game object \"EffectControl\".");
			}
		}

		return (EffectControl.instance);
	}
}