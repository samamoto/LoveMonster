using UnityEngine;
using UnitySampleAssets.CrossPlatformInput;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class ThirdPersonUserControl : MonoBehaviour
{

    public bool walkByDefault = false; // toggle for walking state

    public bool lookInCameraDirection = true;// should the character be looking in the same direction that the camera is facing

    private Vector3 lookPos; // The position that the character should be looking towards
    private ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
    private Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera

    private Vector3 move;
    private bool jump;// the world-relative desired move direction, calculated from the camForward and user input.

    // Use this for initialization
    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        character = GetComponent<ThirdPersonCharacter>();
    }

    void Update()
    {
        if(!jump)
		jump = CrossPlatformInputManager.GetButtonDown("Jump");
	}

	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{
        // read inputs
        bool crouch = false;
        bool slide = false;
        bool vault = false;
        bool climb = false;
        bool wallrun = false;

		//get input from sticks and buttons
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");

		//ToDo:鹿島
		//インプットを作ったやつに変える
		//Read in inputs and set true/false
		// -true only if the button is pressed and the character is in the ActionArea)
		crouch = Input.GetKey(KeyCode.C);
        slide = Input.GetKey(KeyCode.M) && (move.magnitude > 0); 
        vault = Input.GetKey(KeyCode.V);
        climb = Input.GetKey(KeyCode.Z);
        wallrun = Input.GetKey(KeyCode.X) && (move.magnitude > 0);


        // calculate move direction to pass to character
        if (cam != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            move = v*camForward + h*cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = v*Vector3.forward + h*Vector3.right;
        }

        if (move.magnitude > 1) move.Normalize();

        // calculate the head look target position
        lookPos = lookInCameraDirection && cam != null
                        ? transform.position + cam.forward*100
                        : transform.position + transform.forward*100;

        // pass all parameters to the character control script
        character.Move(move, crouch, jump, vault, slide,climb,wallrun, lookPos);
        jump = false;
    }
}
