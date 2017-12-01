using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartPoint : MonoBehaviour {

	public Transform restartPosition;
	PlayerManager m_PlayerMgr;

	/*
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	*/

	private void OnTriggerEnter(Collider other) {

		if(other.tag == "Player") {

			// Playerが初回に触れたらリスタート位置として記録する
			// コンポーネントを記録
			m_PlayerMgr = other.GetComponent<PlayerManager>();
			m_PlayerMgr.setRestartPosition(restartPosition.position);


		}

	}


}
