using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// FragmentのObject一つ一つにつけるScript
/// </summary>

public class BreakBlockStatus : MonoBehaviour {

    public float breakBlockTime;
    float destroyTime;

    // Use this for initialization
    void Start () {
        destroyTime = 50;
    }
	
	// Update is called once per frame
	void Update () {
        Transform myTransform = this.transform;

        if (myTransform.position.y <= -destroyTime)
        {
            deleteObject();
        }
        
    }

    //Explosionのタグに触れている間
    void OnCollisionStay(Collision other)
    {
        if (other.collider.tag.ToString() == "Player")
        {
            breakBlockTime += Time.deltaTime;
        }
    }

    //  オブジェクトデリート
    void deleteObject() { Destroy(this.gameObject); }
}
