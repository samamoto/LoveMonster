using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupActionButton : MonoBehaviour {

	SearchCollider m_Search;

	// Use this for initialization
	void Start () {
		m_Search = transform.parent.GetComponent<SearchCollider>();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
