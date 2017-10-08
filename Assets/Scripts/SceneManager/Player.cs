using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    Rigidbody RB;
    Vector3 velocity;

    // Use this for initialization
    void Start()
    {
        RB = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.x = Input.GetAxis("Horizontal");
        velocity.z = Input.GetAxis("Vertical");

        RB.AddForce(velocity * 20 * Time.deltaTime, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "LargeSquare":
                Debug.Log("LargeSquare");
                break;
            case "SmallSquare":
                Debug.Log("SmallSquare");
                break;
            case "LargeRectAngle":
                Debug.Log("LargeRectAngle");
                break;
            case "SmallRectAngle":
                Debug.Log("SmallRectAngle");
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        switch(other.tag)
        {
            case "LargeSquare":

                break;
            case "SmallSquare":

                break;
            case "LargeRectAngle":

                break;
            case "SmallRectAngle":

                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case "LargeSquare":

                break;
            case "SmallSquare":

                break;
            case "LargeRectAngle":

                break;
            case "SmallRectAngle":

                break;
        }
    }
}
