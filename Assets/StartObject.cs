using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObject : MonoBehaviour {
	public int StartPlayerID { get; private set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getStartPlayerID() { return StartPlayerID; }
}
