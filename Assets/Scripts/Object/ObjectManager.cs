using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

	private static ObjectManager _instance;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	// Singleton
	//------------------------------------------------------------

	/// <summary>
	/// インスタンスの入手
	/// </summary>
	public static ObjectManager Instance {
		get {
			if (_instance == null) _instance = new ObjectManager();

			return _instance;
		}
	}
}
