using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


/**
 * This component moves a player controlled with a CharacterController using the keyboard.
 */
[RequireComponent(typeof(CharacterController))]
public class CharacterKeyboardMover: MonoBehaviour {
    [Tooltip("Speed of player keyboard-movement, in meters/second")]
    [SerializeField] float speed = 3.5f;
    [SerializeField] float gravity = 9.81f;
    [SerializeField] float runMultiplier = 2f;
    
    private CharacterController cc;
    [SerializeField] InputAction moveAction;
    private bool isRunning = false; // Flag to track if the player is running
    private void OnEnable() { moveAction.Enable(); }
    private void OnDisable() { moveAction.Disable(); }
    void OnValidate() {
        // Provide default bindings for the input actions.
        // Based on answer by DMGregory: https://gamedev.stackexchange.com/a/205345/18261
        if (moveAction == null)
            moveAction = new InputAction(type: InputActionType.Button);
        if (moveAction.bindings.Count == 0)
            moveAction.AddCompositeBinding("2DVector")
                .With("Up", "<Keyboard>/upArrow")
                .With("Down", "<Keyboard>/downArrow")
                .With("Left", "<Keyboard>/leftArrow")
                .With("Right", "<Keyboard>/rightArrow");
    }

    void Start() {
        cc = GetComponent<CharacterController>();
    }

    Vector3 velocity = new Vector3(0,0,0);

    void Update()  {
        if (Keyboard.current.leftShiftKey.isPressed)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        float currentSpeed = isRunning ? speed * runMultiplier : speed;

        if (cc.isGrounded)
        {
            Vector3 movement = moveAction.ReadValue<Vector2>(); // Implicitly convert Vector2 to Vector3, setting z=0.
            velocity.x = movement.x * currentSpeed;
            velocity.z = movement.y * currentSpeed;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }

        // Move in the direction you look
        velocity = transform.TransformDirection(velocity);

        cc.Move(velocity * Time.deltaTime);
    }
}
