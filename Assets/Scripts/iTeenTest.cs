using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class iTeenTest : MonoBehaviour
{
    public GameObject obj;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        //iTween.MoveFrom(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));
        //iTween.MoveTo(obj, iTween.Hash("x", obj.transform.position.x+1, "time", 3));
        //iTween.RotateTo(obj, iTween.Hash("y", 90, "time", 3));
        // ついてくる
        //iTween.MoveUpdate(gameObject, iTween.Hash("position", obj.transform.position, "time", 5));
    }
}