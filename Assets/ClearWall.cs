﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearWall : MonoBehaviour {

	public float scrollSpeed_u = 0.5F;
	public float scrollSpeed_v = 0.5F;
	private Renderer rend;

	void Start() {
		rend = GetComponent<Renderer>();
	}

	void Update() {
		UvScroll();


	}

	void UvScroll() {
		float offset_u = Time.time * -scrollSpeed_u;
		float offset_v = Time.time * -scrollSpeed_v;
		rend.material.SetTextureOffset("_MainTex", new Vector2(offset_u, offset_v));
	}

	public void OnTriggerEnter(Collider other) {
		if (other.tag == "Player"){
			EffectControl.get().createMuzzleLaserBolt(other.transform.position, 2);
		}
	}

}
