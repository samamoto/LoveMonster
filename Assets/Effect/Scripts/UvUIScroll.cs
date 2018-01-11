//======================================
/*
@autor ktk.kumamoto
@date 2015.3.6 create
@note UvScroll
*/
//======================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UvUIScroll : MonoBehaviour {
	
	public float scrollSpeed_u = 0.5F;
	public float scrollSpeed_v = 0.5F;
	private CanvasRenderer rend;
	void Start() {
		rend = GetComponent<CanvasRenderer>();
	}
	void Update() {
		float offset_u = Time.time * - scrollSpeed_u;
		float offset_v = Time.time * - scrollSpeed_v; 
		rend.GetMaterial().SetTextureOffset("_MainTex", new Vector2(offset_u, offset_v));
	}
}