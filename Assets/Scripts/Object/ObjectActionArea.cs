﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//============================================================
// ==2017/10/31 Oyama Add
//============================================================
public class ObjectActionArea : MonoBehaviour {

	string m_tagValue;

	protected void Start() {
		m_tagValue = tag;   // タグの名前コピー
	}

	// Update is called once per frame
	void Update () {
		
	}

	// コライダーに当たった
	void OnTriggerEnter(Collider other) {

		Vector3 vec;
		// ぶつかった対象にメッセージ(関数)を送る
		other.SendMessage(tag, m_tagValue);
		if (other.tag == "Player") {
			vec = other.GetComponent<Transform>().position;

		}


	}

	// コライダーに当たってる
	void OnTriggerStay(Collider other) {

	}
}
