using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

	private static EventManager _instance;	// インスタンス

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "LargeSquare":

                break;
        }
    }


	// Singleton
	//------------------------------------------------------------
	//private EventManager() {
	//	Debug.Log("Create SampleSingleton instance.");
	//}

	/// <summary>
	/// インスタンスの入手
	/// </summary>
	public static EventManager Instance {
		get {
			if (_instance == null) _instance = new EventManager();

			return _instance;
		}
	}



}
