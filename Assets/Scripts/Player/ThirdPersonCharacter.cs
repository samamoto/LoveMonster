using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Controller;

[SelectionBase]//Scene�r���[�ŃL�����N�^��I�������Ƃ��ɂ��̃X�N���v�g�̓����Ă���q�G�����L�[���D�悵�đI�������悤�ɂȂ�
public class ThirdPersonCharacter : MonoBehaviour
{

    [SerializeField] private float jumpPower = 12; // determines the jump force applied when jumping (and therefore the jump height)
    [SerializeField] private float airSpeed = 6; // determines the max speed of the character while airborne
    [SerializeField] private float airControl = 2; // determines the response speed of controlling the character while airborne
    [Range(1, 4)] [SerializeField] public float gravityMultiplier = 2; // gravity modifier - often higher than natural gravity feels right for game characters
    [SerializeField] [Range(0.1f, 3f)] private float moveSpeedMultiplier = 1; // how much the move speed of the character will be multiplied by
    [SerializeField] [Range(0.1f, 3f)] private float animSpeedMultiplier = 1; // how much the animation of the character will be multiplied by
    [SerializeField] private AdvancedSettings advancedSettings; // Container for the advanced settings class , thiss allows the advanced settings to be in a foldout in the inspector


    [System.Serializable]
    public class AdvancedSettings
    {
		//public float stationaryTurnSpeed = 180; // additional turn speed added when the player is stationary (added to animation root rotation)
		public float stationaryTurnSpeed = 540; // additional turn speed added when the player is stationary (added to animation root rotation)
		//public float movingTurnSpeed = 360; // additional turn speed added when the player is moving (added to animation root rotation)
		public float movingTurnSpeed = 1080; // additional turn speed added when the player is moving (added to animation root rotation)
		public float headLookResponseSpeed = 2; // speed at which head look follows its target
        public float crouchHeightFactor = 0.6f; // collider height is multiplied by this when crouching
        public float crouchChangeSpeed = 4; // speed at which capsule changes height when crouching/standing
        public float autoTurnThresholdAngle = 100; // character auto turns towards camera direction if facing away by more than this angle
		//public float autoTurnSpeed = 2; // speed at which character auto-turns towards cam direction
		public float autoTurnSpeed = 6; // speed at which character auto-turns towards cam direction
		public PhysicMaterial zeroFrictionMaterial; // used when in motion to enable smooth movement
        public PhysicMaterial highFrictionMaterial; // used when stationary to avoid sliding down slopes
		//public float jumpRepeatDelayTime = 0.25f; // amount of time that must elapse between landing and being able to jump again
		public float jumpRepeatDelayTime = 0.1f; // amount of time that must elapse between landing and being able to jump again
		public float runCycleLegOffset = 0.2f; // animation cycle offset (0-1) used for determining correct leg to jump off
		public float groundStickyEffect = 5f; // power of 'stick to ground' effect - prevents bumping down slopes.
    }

    public Transform lookTarget { get; set; } // The point where the character will be looking at

    public LayerMask groundCheckMask;			// ������C���X�y�N�^����Everything��
    public LayerMask crouchCheckMask;			// �ݒ肵�Ȃ���OnGround�������Ȃ��C������B����AddComponent����Ə����l��Nothing�ɂȂ� oyama add

    // david changed OnGround to public
    [SerializeField] public bool onGround; // Is the character on the ground
    private Vector3 currentLookPos; // The current position where the character is looking
    private float originalHeight; // Used for tracking the original height of the characters capsule collider
    private Animator animator; // The animator for the character
    private float lastAirTime; // USed for checking when the character was last in the air for controlling jumps
    private CapsuleCollider capsule; // The collider for the character
    private const float half = 0.5f; // whats it says, it's a constant for a half
    private Vector3 moveInput;
    private bool crouchInput;
    private bool jumpInput;

	// David Add
	private bool slideInput;

	private float turnAmount;
    private float forwardAmount;
    private Vector3 velocity;
    private IComparer rayHitComparer;
    public float lookBlendTime;
    public float lookWeight;


    /// �ǉ��F���c
    /// �A�j���[�V�����̃p�����[�^
    private bool vaultInput;
    private bool climbInput;
    private bool WallRunInput;

    // david add
    public bool wallRunning; // are we wall running

    //private MoveState m_MoveState;	// �ړ�������C����

    //ToDo:�^�[�Q�b�g�I�u�W�F�N�g�擾�p�p�����[�^
    public Vector3 Target;

    //Action�̌v�Z����
    ActionVault MA_Vault = new ActionVault();
    ActionSlide MA_Slide = new ActionSlide();
    ActionClimb MA_Climb = new ActionClimb();
    ActionWallRun MA_WallRun = new ActionWallRun();

	/// end

	// Use this for initialization
	void Start()
    {
        //david add
        wallRunning = false;

        animator = GetComponentInChildren<Animator>();
        capsule = GetComponent<Collider>() as CapsuleCollider;

        // as can return null so we need to make sure thats its not before assigning to it
        if (capsule == null)
        {
            Debug.LogError(" collider cannot be cast to CapsuleCollider");
        }
        else
        {
            originalHeight = capsule.height;
            capsule.center = Vector3.up*originalHeight*half;
        }

        MA_Vault.Start();
        MA_Slide.Start();
        MA_Climb.Start();
        MA_WallRun.Start();

        rayHitComparer = new RayHitComparer();

        SetUpAnimator();

        // give the look position a default in case the character is not under control
        currentLookPos = Camera.main.transform.position;

	}

	// Lookweight is probably used to determine how fast character turns etc.
	IEnumerator BlendLookWeight()
    {
        float t = 0f;
        while (t < lookBlendTime)
        {
            lookWeight = t / lookBlendTime;
            t += Time.deltaTime;
            yield return null;
        }
        lookWeight = 1f;
    }

    // Probably the look weight enabling so the function would run if designated to
    void OnEnable()
    {
        if (lookWeight == 0f)
        {
            StartCoroutine(BlendLookWeight());
        }
    }

    // The Move function is designed to be called from a separate component
    // based on User input, or an AI control script
    public void Move(Vector3 move, bool crouch, bool jump, bool vault, bool slide, bool climb, bool wallrun, Vector3 lookPos)
    {

        if (move.magnitude > 1) move.Normalize();

        // transfer input parameters to member variables.
        // David Add
        this.moveInput = move;
        this.crouchInput = crouch;
        this.jumpInput = jump;
        this.vaultInput = vault;
        this.slideInput = slide;
        this.climbInput = climb;
        this.WallRunInput = wallrun;
        this.currentLookPos = lookPos;

        // grab current velocity, we will be changing it.
        velocity = GetComponent<Rigidbody>().velocity;

        ConvertMoveInput(); // converts the relative move vector into local turn & fwd values

        //TurnTowardsCameraForward(); // makes the character face the way the camera is looking

        PreventStandingInLowHeadroom(); // so the character's head doesn't penetrate a low ceiling

        ScaleCapsuleForCrouching(); // so you can fit under low areas when crouching

        ApplyExtraTurnRotation(); // this is in addition to root rotation in the animations

        // if wallRunning we want to handle gravity with the wallrun code
        if (!wallRunning)
        {
            GroundCheck(); // detect and stick to ground

            if (onGround)
            {
                HandleGroundedVelocities();
            }
            else
            {
                HandleAirborneVelocities();
            }
        }

        SetFriction(); // use low or high friction values depending on the current state


		// David Add
		/*
		if (vault)
        {
            MA_Vault.Update();
            //Vault(ConstAnimationStateTags.PlayerStateVault);
        }
        if(slide)
        {

            MA_Slide.Update();
        }
        if(climb)
        {

            MA_Climb.Update();
        }
        if(wallrun)
        {

            MA_WallRun.Update();
        }
		*/
        // control and velocity handling is different when grounded and airborne:

        UpdateAnimator(); // send input and other state parameters to the animator
        
        // reassign velocity, since it will have been modified by the above functions.
        GetComponent<Rigidbody>().velocity = velocity;


    }

    private void ConvertMoveInput()
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction. 
        Vector3 localMove = transform.InverseTransformDirection(moveInput);
        turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        forwardAmount = localMove.z;
    }

    private void TurnTowardsCameraForward()
    {
        // automatically turn to face camera direction,
        // when not moving, and beyond the specified angle threshold
        if (Mathf.Abs(forwardAmount) < .01f)
        {
            Vector3 lookDelta = transform.InverseTransformDirection(currentLookPos - transform.position);
            float lookAngle = Mathf.Atan2(lookDelta.x, lookDelta.z)*Mathf.Rad2Deg;

            // are we beyond the threshold of where need to turn to face the camera?
            if (Mathf.Abs(lookAngle) > advancedSettings.autoTurnThresholdAngle)
            {
                turnAmount += lookAngle*advancedSettings.autoTurnSpeed*.001f;
            }
        }
    }

    private void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!crouchInput)
        {
            Ray crouchRay = new Ray(GetComponent<Rigidbody>().position + Vector3.up*capsule.radius*half, Vector3.up);
            float crouchRayLength = originalHeight - capsule.radius*half;
            if (Physics.SphereCast(crouchRay, capsule.radius*half, crouchRayLength , crouchCheckMask))
            {
                crouchInput = true;
            }
        }
    }

    private void ScaleCapsuleForCrouching()
    {
        // scale the capsule collider according to
        // if crouching ...
        if (onGround && crouchInput && (capsule.height != originalHeight*advancedSettings.crouchHeightFactor))
        {
            capsule.height = Mathf.MoveTowards(capsule.height, originalHeight*advancedSettings.crouchHeightFactor,
                                                Time.deltaTime*4);
            capsule.center = Vector3.MoveTowards(capsule.center,
                                                    Vector3.up*originalHeight*advancedSettings.crouchHeightFactor*half,
                                                    Time.deltaTime*2);
        }
        //TODO: I added this forcefully, should make separate function or put into slide function
        //for if sliding
        else if (onGround && slideInput && (capsule.height != originalHeight * advancedSettings.crouchHeightFactor))
        {
            //capsule.height = Mathf.MoveTowards(capsule.height, originalHeight * advancedSettings.crouchHeightFactor,
            //                                   Time.deltaTime * 4);
            //capsule.center = Vector3.MoveTowards(capsule.center,
            //                                     Vector3.up * originalHeight * advancedSettings.crouchHeightFactor * half,
            //                                     Time.deltaTime * 2);
        }
        // ... everything else 
        else if (capsule.height != originalHeight && capsule.center != Vector3.up*originalHeight*half)
        {
            capsule.height = Mathf.MoveTowards(capsule.height, originalHeight, Time.deltaTime*4);
            capsule.center = Vector3.MoveTowards(capsule.center, Vector3.up*originalHeight*half, Time.deltaTime*2);
        }
    }

    private void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(advancedSettings.stationaryTurnSpeed, advancedSettings.movingTurnSpeed,
                                        forwardAmount);
        transform.Rotate(0, turnAmount*turnSpeed*Time.deltaTime, 0);
    }

    private void GroundCheck()
    {
        Ray ray = new Ray(transform.position + Vector3.up*.1f, -Vector3.up);
        RaycastHit[] hits = Physics.RaycastAll(ray, .5f,groundCheckMask);
        System.Array.Sort(hits, rayHitComparer);

        
        if (velocity.y < jumpPower * .5f)
        {
            onGround = false;
            GetComponent<Rigidbody>().useGravity = true;
            foreach (var hit in hits)
            {
                // check whether we hit a non-trigger collider (and not the character itself)
                if (!hit.collider.isTrigger)
                {
                    // this counts as being on ground.

                    // stick to surface - helps character stick to ground - specially when running down slopes
                    if (velocity.y <= 0)
                    {
                        GetComponent<Rigidbody>().position = Vector3.MoveTowards(GetComponent<Rigidbody>().position, hit.point,
                                                                    Time.deltaTime * advancedSettings.groundStickyEffect);
                    }

                    onGround = true;
                    GetComponent<Rigidbody>().useGravity = false;
                    break;
                }
            }
        }

        animator.SetBool("is_Grounded", onGround);// oyama add

        // remember when we were last in air, for jump delay
        if (!onGround) lastAirTime = Time.time;

    }

    private void SetFriction()
    {

        if (onGround)
        {

            // set friction to low or high, depending on if we're moving
            if (moveInput.magnitude == 0)
            {
                // when not moving this helps prevent sliding on slopes:
                GetComponent<Collider>().material = advancedSettings.highFrictionMaterial;
            }
            else
            {
                // but when moving, we want no friction:
                GetComponent<Collider>().material = advancedSettings.zeroFrictionMaterial;
            }
        }
        else
        {
            // while in air, we want no friction against surfaces (walls, ceilings, etc)
            GetComponent<Collider>().material = advancedSettings.zeroFrictionMaterial;
        }
    }

    private void HandleGroundedVelocities()
    {

        velocity.y = 0;

        if (moveInput.magnitude == 0)
        {
            // when not moving this prevents sliding on slopes:
            velocity.x = 0;
            velocity.z = 0;
        }
		// check whether conditions are right to allow a jump:
		//bool animationGrounded = animator.GetCurrentAnimatorStateInfo(0).IsName("is_Grounded");
		bool animationGrounded = animator.GetBool("is_Grounded");
        bool okToRepeatJump = Time.time > lastAirTime + advancedSettings.jumpRepeatDelayTime;

        if (jumpInput && !crouchInput && okToRepeatJump && animationGrounded)
        {
            // jump!
            onGround = false;
            velocity = moveInput*airSpeed;
            velocity.y = jumpPower;
        }
    }

    private void HandleAirborneVelocities()
    {
        // we allow some movement in air, but it's very different to when on ground
        // (typically allowing a small change in trajectory)
        Vector3 airMove = new Vector3(moveInput.x*airSpeed, velocity.y, moveInput.z*airSpeed);
        velocity = Vector3.Lerp(velocity, airMove, Time.deltaTime*airControl);
        GetComponent<Rigidbody>().useGravity = true;

        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity*gravityMultiplier) - Physics.gravity;
        GetComponent<Rigidbody>().AddForce(extraGravityForce);

    }

    private void UpdateAnimator()
    {
        // Here we tell the animator what to do based on the current states and inputs.

        // only use root motion when on ground:
        animator.applyRootMotion = onGround;

        // update the animator parameters
        animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
        animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
        animator.SetBool("is_Crouch", crouchInput);
        animator.SetBool("is_Slide", slideInput);
        animator.SetBool("is_Vault", vaultInput);
        animator.SetBool("is_Climb", climbInput);
        animator.SetBool("is_WallRun", WallRunInput);
        animator.SetBool("is_Grounded", onGround);

        if (!onGround)
        {
            animator.SetFloat("JumpPower", velocity.y);
        }

        // calculate which leg is behind, so as to leave that leg trailing in the jump animation
        // (This code is reliant on the specific run cycle offset in our animations,
        // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
        float runCycle =
            Mathf.Repeat(
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime + advancedSettings.runCycleLegOffset, 1);
        float jumpLeg = (runCycle < half ? 1 : -1)*forwardAmount;
        if (onGround)
        {
            animator.SetFloat("JumpLeg", jumpLeg);
        }

        // the anim speed multiplier allows the overall speed of walking/running to be tweaked in the inspector,
        // which affects the movement speed because of the root motion.
        if (onGround && moveInput.magnitude > 0)
        {
            animator.speed = animSpeedMultiplier;
        }
        else
        {
            // but we don't want to use that while airborne
            animator.speed = 1;
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        // we set the weight so most of the look-turn is done with the head, not the body.
        animator.SetLookAtWeight(lookWeight, 0.2f, 2.5f);

        // if a transform is assigned as a look target, it overrides the vector lookPos value
        if (lookTarget != null)
        {
            currentLookPos = lookTarget.position;
        }

        // Used for the head look feature.
        animator.SetLookAtPosition(currentLookPos);
    }


    private void SetUpAnimator()
    {
        // this is a ref to the animator component on the root.
        animator = GetComponent<Animator>();

        // we use avatar from a child animator component if present
        // (this is to enable easy swapping of the character model as a child node)
        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != animator)
            {
                animator.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
    }

    public void OnAnimatorMove()
    {
        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        GetComponent<Rigidbody>().rotation = animator.rootRotation;
        if (onGround && Time.deltaTime > 0)
        {
            Vector3 v = (animator.deltaPosition*moveSpeedMultiplier)/Time.deltaTime;

            // we preserve the existing y part of the current velocity.
            v.y = GetComponent<Rigidbody>().velocity.y;
            GetComponent<Rigidbody>().velocity = v;
        }
    }


    void OnDisable()
    {
        lookWeight = 0f;
    }

    //used for comparing distances
    private class RayHitComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return ((RaycastHit) x).distance.CompareTo(((RaycastHit) y).distance);
        }
    }

    /// <summary>
    /// ���g���������Ƃ��ɓ����蔻�������������
    /// </summary>
    void HulfCollider()
    {
        capsule.height = Mathf.MoveTowards(capsule.height, originalHeight * advancedSettings.crouchHeightFactor,Time.deltaTime * 4);
        capsule.center = Vector3.MoveTowards(capsule.center,Vector3.up * originalHeight * advancedSettings.crouchHeightFactor * half,Time.deltaTime * 2);
    }

}