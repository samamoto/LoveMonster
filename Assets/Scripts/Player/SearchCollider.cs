using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エリアに入ったものの情報を取得する
/// </summary>

//RequireComponentで指定することでそのスクリプトが必須であることを示せる
[RequireComponent(typeof(PlayerManager))]
public class SearchCollider : MonoBehaviour {

	// Mask
	public LayerMask mask;

	// Ray
	private Ray m_DwRay;                    // 下に飛ばしてJump中の情報などを得る
	public float groundDist;				// 下までの距離
	public float groundDistMax = 10.0f;    // 探知距離の長さ

	// 取得可能なDictionary
	//!GameObjectのUniqueな値intと、距離floatを取り、齟齬がないように管理する
	public Dictionary<int, float> m_DistDic = new Dictionary<int, float>();    // オブジェクトまでの距離近い順に並べ替える
#if DEBUG
	public List<string> dbg_DistDic;
#endif
	// Use this for initialization
	private void Start() {
		// プレイヤーの下側にRayを出す
		m_DwRay = new Ray(transform.parent.position+new Vector3(0f,0.45f,0f),Vector3.down);
#if DEBUG
		dbg_DistDic = new List<string>();
#endif
	}

	// Update is called once per frame
	private void Update() {
		//transform.position = transform.parent.position + new Vector3(0f, 0.45f, 0f);
		RaycastHit hit = new RaycastHit();
		m_DwRay = new Ray(transform.parent.position + new Vector3(0f, 0.1f, 0f), Vector3.down);

		// Rayが衝突したかを検知する
		if (Physics.Raycast(m_DwRay, out hit, groundDistMax)) {
			groundDist = hit.distance;
#if DEBUG
			Debug.Log(groundDist);
			Debug.Log(m_DwRay.origin);
			Debug.Log(hit.transform.ToString());
			Debug.Log(hit.transform.tag);
#endif
		}
#if DEBUG
		Debug.DrawRay(transform.position, m_DwRay.direction, Color.red);
#endif
	}

	//============================================================
	// Function
	//============================================================

	//============================================================
	// Collision
	//============================================================
	private void OnTriggerEnter(Collider other) {
		// 親のプレイヤーを基準に、エリアに入った物の距離を全て取得し、格納していく
		// ただしオブジェクト名にAction Areaがついていたら
		int id = other.GetInstanceID();
		if (other.name == "ActionArea") {
			m_DistDic.TryGetValueEx(id, 0.0f);	// 先にNullと初期値格納
			m_DistDic[id] = Vector3.Distance(transform.parent.position, other.transform.position);  // 距離を格納
#if DEBUG
			//Debug.Log(m_DistDic[id].ToString());
			KeyValuePair<int, float> pair = new KeyValuePair<int, float>(id, m_DistDic[id]);
			dbg_DistDic.Add(pair.ToString());
#endif
		}
	}

	private void OnTriggerStay(Collider other) {
		// 距離の情報を更新
		int id = other.GetInstanceID();
		m_DistDic[id] = Vector3.Distance(transform.parent.position, other.transform.position);  // 距離を更新
	}

	private void OnTriggerExit(Collider other) {
		int id = other.GetInstanceID();
		// 取得した情報を削除する keyがnullならfalse
		if (m_DistDic.ContainsKeyNullable(id) != false) {
			m_DistDic.Remove(id);
		}
	}

	//============================================================
	// getter/setter
	//============================================================
	/**
	 * @brief <summary>地上までの距離を取得する</summary>
	 * @return 距離
	 */

	//public float GetGroundDistance() {
	//	return groundDist;
	//}

	/**
	 * @brief <summary>オブジェクトまでの最短距離を取得</summary>
	 * @return 距離
	 */
	 public float GetObjDistance() {
		float min = 10.0f;
		foreach(KeyValuePair<int, float> pair in m_DistDic) {
			if(pair.Value < min) {
				min = pair.Value;
			}
		}
		return min;
	}

	/**
	 * @brief <summary>オブジェクトまでの距離を全て取得</summary>
	 * @return 全ての距離（配列）
	 */
	public float[] GetObjDistances() {
		List<float> dist = new List<float>();
		foreach (KeyValuePair<int, float> pair in m_DistDic) {
			dist.Add(pair.Value);
		}
		return dist.ToArray();
	}


}
/**
 * @fn
 * ここに関数の説明を書く
 * @brief <summary>要約説明</summary>
 * @param (引数名) 引数の説明
 * @param (引数名) 引数の説明
 * @return 戻り値の説明
 * @sa 参照すべき関数を書けばリンクが貼れる
 * @detail 詳細な説明
 */
