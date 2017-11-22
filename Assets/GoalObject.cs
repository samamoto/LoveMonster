using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 触れたらゴール
/// </summary>
public class GoalObject : MonoBehaviour {

	 // Use this for initialization
	 void Start () {
		
	 }
	
	 // Update is called once per frame
	 void Update () {
		
	 }

	 void OnTriggerStay(Collider other) {

		  SceneChange.Instance._SceneLoadResult();

	 }
}
