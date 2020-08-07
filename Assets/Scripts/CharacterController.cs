using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    public Animator Animator { get { return animator; } }
    [SerializeField]
    private float DirectionDampTime = .25f;
    [SerializeField]
    private float directionSpeed = 1.5f;

    [SerializeField]
    private ThirdPersonCamera gamecam;
    [SerializeField]
    private float rotationDegreesPerSecond = 120f;
    [SerializeField]
    private float speedDamp = 0.05f;
    [SerializeField]
    private float jumpMultiplier = 0.2f;
    [SerializeField]
    private CapsuleCollider capCollider;
    [SerializeField]
    private float jumpDist = 1f;
    [SerializeField]
    private Collider interactCollider;

    public Transform cameraTarget;

    private float speed = 0.0f;
    private float direction = 0.0f;
    private float horizontal = 0.0f;
    private float vertical = 0.0f;
    private float angle = 0.0f;
    private float capsuleHeight;

    public float Vertical {
        get {
            return this.vertical;
        }
    }

    public float Horizontal {
        get {
            return this.horizontal;
        }
    }

    public float Speed {
        get {
            return this.speed;
        }
    }

    public float Direction {
        get { 
            return this.direction;
        }
    }

    public float LocomotionThreshold {
        get {
            return 0.4f;
        }
    }

    private Rigidbody rigidBody;
    private AnimatorStateInfo stateInfo;
    private AnimatorTransitionInfo transitionInfo;

    
    private int locomotionId = 0;
    private int locomotionPivotLId = 0;
    private int locomotionPivotRId = 0;
    private int locomotionPivotLTransId = 0;
    private int locomotionPivotRTransId = 0;
    private bool jump = false;
    private bool interacting = false;

    void Awake() {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    
    void Start()
    {
        capCollider = GetComponent<CapsuleCollider>();
        capsuleHeight = capCollider.height;

        if (animator.layerCount >= 2) 
        {
            animator.SetLayerWeight(1, 1);
        }
        locomotionId = Animator.StringToHash("Base Layer.Locomotion");
        locomotionPivotLId = Animator.StringToHash("Base Layer.LocomotionPivotL");
        locomotionPivotRId = Animator.StringToHash("Base Layer.LocomotionPivotR");
        locomotionPivotLTransId = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.LocomotionPivotL");
        locomotionPivotRTransId = Animator.StringToHash("Base Layer.Locomotion -> Base Layer.LocomotionPivotR");
    }

    // Update is called once per frame
    void Update()
    {

        if (animator) {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            transitionInfo = animator.GetAnimatorTransitionInfo(0);

            animator.SetBool("Jump", jump);


            angle = 0f;
            direction = 0f;
            TranslateStickToWorldspace(GetComponent<Transform>(), gamecam.GetComponent<Transform>(), ref direction, ref speed, ref angle, IsInPivot());

            animator.SetFloat("Speed", speed, speedDamp, Time.deltaTime);
            animator.SetFloat("Direction", direction, DirectionDampTime, Time.deltaTime);

            if (speed > LocomotionThreshold)
            {
                if (!IsInPivot())
                {
                    animator.SetFloat("Angle", angle);
                }
            }
            if (speed < LocomotionThreshold)
            {
                animator.SetFloat("Direction", 0f);
                animator.SetFloat("Angle", 0f);
            }
        }
    }

    void FixedUpdate()
    {
        interactCollider.enabled = interacting;
        if (Mathf.Abs(horizontal) < 0.001f)
        {
            rigidBody.angularVelocity = Vector3.zero;
        }

        if (IsInLocomotion() && (vertical > 0.001f || vertical < -0.001f) && ((direction >= 0 && horizontal >= 0.001f) || (direction < 0 && horizontal < 0.001f)))
        {
            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, rotationDegreesPerSecond * Mathf.Sign(horizontal), 0), Mathf.Abs(horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            this.transform.rotation = (this.transform.rotation * deltaRotation);
        }

        if (IsInJump())
        {
            float oldY = transform.position.y;
            transform.Translate(Vector3.up * jumpMultiplier * animator.GetFloat("JumpCurve"));
            if (IsInLocomotionJump())
            {
                transform.Translate(Vector3.forward * Time.deltaTime * jumpDist);
            }
            capCollider.height = capsuleHeight + (animator.GetFloat("CapsuleCurve") * 0.5f);
            gamecam.transform.Translate(Vector3.up * (transform.position.y - oldY));
        }
    }

    void TranslateStickToWorldspace(Transform root, Transform camera, ref float directionOut, ref float speedOut, ref float angleOut, bool isPivoting)
    {
        Vector3 rootDirection = root.forward;

        Vector3 stickDirection = new Vector3(horizontal, 0, vertical);

        speedOut = stickDirection.sqrMagnitude;

        // Get camera rotation
        Vector3 CameraDirection = camera.forward;
        CameraDirection.y = 0.0f; // kill Y
        Quaternion referentialShift = Quaternion.FromToRotation(Vector3.forward, Vector3.Normalize(CameraDirection));

        // Convert joystick input in Worldspace coordinates
        Vector3 moveDirection = referentialShift * stickDirection;
        Vector3 axisSign = Vector3.Cross(moveDirection, rootDirection);

        float angleRootToMove = Vector3.Angle(rootDirection, moveDirection) * (axisSign.y >= 0 ? -1f : 1f);
        if (!isPivoting)
        {
            angleOut = angleRootToMove;
        }
        angleRootToMove /= 180f;

        directionOut = angleRootToMove * directionSpeed;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        jump = ctx.ReadValueAsButton();
    }

    public bool IsInIdleJump()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.IdleJump");
    }

    public bool IsInLocomotionJump()
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LocomotionJump");
    }

    public bool IsInPivot()
    {
        return stateInfo.nameHash == locomotionPivotLId ||
            stateInfo.nameHash == locomotionPivotRId ||
            transitionInfo.nameHash == locomotionPivotLTransId ||
            transitionInfo.nameHash == locomotionPivotRTransId;
    }

    public bool IsInLocomotion()
    {
        return stateInfo.nameHash == locomotionId;
    }


    public bool IsInJump()
    {
        return (IsInIdleJump() || IsInLocomotionJump());
    }


    public void OnMoveUpdate(InputAction.CallbackContext ctx) {
        Vector2 axis = ctx.ReadValue<Vector2>();
        horizontal = axis.x;
        vertical = axis.y;
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        interacting = ctx.ReadValueAsButton();
    }
}
