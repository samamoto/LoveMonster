using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockControl : MonoBehaviour {

    public GameObject BlockPrefab;
    private float countTime;
    public float revivalTime;
    private bool revivalFlag;


    // Use this for initialization
    void Start()
    {
        revivalFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (revivalFlag)
        {
            if (countTime >= revivalTime)
            {
                Instantiate(BlockPrefab);
                revivalFlag = false;
                countTime = 0;
            }
            else {
                countTime += Time.deltaTime;
            }
            
        }
    }

    public void _BlockRevivalOn()
    {
        revivalFlag = true;
    }
}
