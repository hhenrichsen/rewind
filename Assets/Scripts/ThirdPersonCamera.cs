using UnityEngine;
using UnityEngine.InputSystem;


struct CameraPosition 
{
    private Vector3 position;
    private Transform transform;
    public Vector3 Position { get { return position; } set { position = value; }}
    public Transform Transform { get { return transform; } set { transform = value; }}

    public void Init(string camName, Vector3 position, Transform transform, Transform parent) {
        this.position = position;
        this.transform = transform;
        this.transform.name = camName;
        this.transform.parent = parent;
        this.transform.localPosition = Vector3.zero;
        this.transform.localPosition = position;
    }
}
public class ThirdPersonCamera : MonoBehaviour {
    [SerializeField]
    private CameraManager cameraManager;
    [SerializeField]
    private float distanceAway;
    [SerializeField]
    private float distanceUp;
    [SerializeField]
    private float smooth;
    [SerializeField]
    private Transform followTransform;
    [SerializeField]
    private Vector3 targetPosition;
    [SerializeField]
    private float camSmoothDampTime = 0.1f;
    [SerializeField]
    private float lookDirectionDampTime = 0.1f;
    [SerializeField]
    private float firstPersonLookSpeed = 1.0f;
    [SerializeField]
    private float firstPersonRotationSpeed = 90f;
    [SerializeField]
    private Vector2 firstPersonXAxisClamp;
    [SerializeField]
    private CharacterController character;

    // Positioning
    private Vector3 velocityCamSmooth = Vector3.zero;
    private Vector3 velocityLookDirection = Vector3.zero;
    private Vector3 offset = new Vector3(0.0f, 1.5f, 0.0f);
    private CameraPosition firstPersonCameraPos;

    // Target
    private Vector3 lookDirection;
    private Vector3 currentLookDirection;
    private float xAxisRotation;

    // Controls
    private float horizontal;
    private float vertical;

    void Awake() {
        followTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Start() {
        firstPersonCameraPos = new CameraPosition();
        firstPersonCameraPos.Init(
            "First Person Camera",
            new Vector3(0.0f, 1.6f, 0.2f),
            new GameObject().transform,
            character.transform
        );
    }

    public void LateUpdate() {
        Vector3 characterOffset = followTransform.position + offset;
        Vector3 lookAt = characterOffset;
        targetPosition = Vector3.zero;

        if (cameraManager.Player) {
            ResetCamera();

            if (character.Speed > character.LocomotionThreshold && character.IsInLocomotion()) {
                lookDirection = Vector3.Lerp(followTransform.right * -Mathf.Sign(horizontal), followTransform.forward * Mathf.Sign(vertical), Mathf.Abs(Vector3.Dot(transform.forward, followTransform.forward)));
                Debug.DrawRay(transform.position, lookDirection, Color.white);
            
                currentLookDirection = Vector3.Normalize(characterOffset - transform.position);
                currentLookDirection.y = 0;
            
                currentLookDirection = Vector3.SmoothDamp(currentLookDirection, lookDirection, ref velocityLookDirection, lookDirectionDampTime);
            }

            targetPosition = characterOffset + followTransform.up * distanceUp - Vector3.Normalize(currentLookDirection) * distanceAway;
            lookDirection = characterOffset;
            Debug.DrawLine(followTransform.position, targetPosition, Color.magenta);
        }
        else if (cameraManager.Camera) {
            
            // Calculate rotation amount
            xAxisRotation -= vertical * 0.5f * firstPersonLookSpeed;
            xAxisRotation = Mathf.Clamp(xAxisRotation, firstPersonXAxisClamp.x, firstPersonXAxisClamp.y);
            firstPersonCameraPos.Transform.localRotation = Quaternion.Euler(xAxisRotation, 0, 0);

            // Superimpose character rotation
            Quaternion rotationShift = Quaternion.FromToRotation(this.transform.forward, firstPersonCameraPos.Transform.forward);
            this.transform.rotation = rotationShift * this.transform.rotation;

            Vector3 rotationAmount = Vector3.Lerp(Vector3.zero, new Vector3(0f, firstPersonLookSpeed * firstPersonRotationSpeed * Mathf.Sign(horizontal), 0f), Mathf.Abs(horizontal));
            Quaternion deltaRotation = Quaternion.Euler(rotationAmount * Time.deltaTime);
            character.transform.rotation = (character.transform.rotation * deltaRotation);

            targetPosition = firstPersonCameraPos.Transform.position;
            lookAt = (Vector3.Lerp(this.transform.position + this.transform.forward, lookDirection, Vector3.Distance(this.transform.position, firstPersonCameraPos.Transform.position)));
        }
        // character.Animator.SetLookAtWeight(lookWeight);

        CompoensateForWalls(characterOffset, ref targetPosition);
        smoothPosition(transform.position, targetPosition);

        this.transform.LookAt(lookAt);
    }


    public void ResetCamera() {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime);
    }

    public void ResetPosition()
    {
        transform.position = character.cameraTarget.position;
        transform.rotation = character.cameraTarget.rotation;
    }

    public void OnPlayer(InputAction.CallbackContext ctx) {
        ResetCamera();
    }

    public void OnMove(InputAction.CallbackContext ctx) {
        Vector2 stick = ctx.ReadValue<Vector2>();
        horizontal = stick.x;
        vertical = stick.y;
    }

    private void smoothPosition(Vector3 fromPos, Vector3 toPos) {
        this.transform.position = Vector3.SmoothDamp(fromPos, toPos, ref velocityCamSmooth, camSmoothDampTime);
    }

    private void CompoensateForWalls(Vector3 fromObject, ref Vector3 toTarget) {
        RaycastHit wallHit = new RaycastHit();
        if (Physics.Linecast(fromObject, toTarget, out wallHit)) {
            toTarget = new Vector3(wallHit.point.x, toTarget.y, wallHit.point.z);
        }
    }
}