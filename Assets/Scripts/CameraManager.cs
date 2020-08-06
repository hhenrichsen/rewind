using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CameraManager : MonoBehaviour {

    private PlayerInput playerInput;

    public bool Camera { get; private set; }
    public bool Player { get; private set; }

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
        Player = true;
    }

    public void Pause()
    {
        playerInput.SwitchCurrentActionMap("Paused");
        Time.timeScale = 0.0f;
    }

    public void Unpause()
    {
        playerInput.SwitchCurrentActionMap("Player");
        Time.timeScale = 1.0f;
    }

    public void ToCamera(InputAction.CallbackContext ctx) {
        playerInput.SwitchCurrentActionMap("Camera");
        Camera = true;
        Player = false;
    }

    public void ToPlayer(InputAction.CallbackContext ctx) {
        playerInput.SwitchCurrentActionMap("Player");
        Camera = false;
        Player = true;
    }
}