using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Controller;

// A compilation of convenient functions for debugging and testing purposes
public class DebugTestFunctions : MonoBehaviour {

    private Controller.Controller m_Controller;
    public Transform[] StartLocations;
    
    private int locationNum;

	// Use this for initialization
	void Start () {
        m_Controller = GetComponent<Controller.Controller>();

        // Make sure there are locations to teleport to
        if (StartLocations.Length > 0)
        {
            locationNum = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
    }

    void LateUpdate()
    {
        // Upon pressing the "Menu" button we teleport the player
        if (m_Controller.GetButtonDown(Button.LStick))
        {
            // cycle back to first location after teleporting to the last 1
            if (locationNum == StartLocations.Length)
            {
                locationNum = 1;
            }
            else
            {
                locationNum++;
            }

            ReturnToStart();
        }

        if (m_Controller.GetButtonDown(Button.View))
        {
            ReturnToStart();
        }
    }

    // Teleports player to a set location determined by StartLocations
    // "View" button teleports player to currently set location
    // "Left Stick Button" sets teleport location to the next location and teleports player to it
    // If the location is invalid an error message will display
    private void ReturnToStart()
    {        
        if(StartLocations[locationNum - 1])
        {
            transform.position = StartLocations[locationNum - 1].position;
        }
        else
        {
            Debug.Log("Teleport location is invalid");
        }
    }
}
