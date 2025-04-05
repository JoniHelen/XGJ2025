using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ElevatorInputHandler : MonoBehaviour {
    [SerializeField] private Gun gun;
    [SerializeField] private Plunger plunger;
    [SerializeField] private PlayerInput playerInput;
    
    private Vector2 _input;
    private Camera _camera;
    
    void Start() {
        _camera = Camera.main;
    }

    void Update() {
        gun.transform.up = _processedInput;
    }

    public void OnPlayerDeath() {
        playerInput.SwitchCurrentActionMap("Dead");
    }

    public void OnLook(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        
        var raw = ctx.ReadValue<Vector2>();
        
        if (raw.sqrMagnitude < 0.3f) return; // Stick dead zone
        
        _input = ctx.control.device.deviceId == Mouse.current.deviceId ? 
            _camera.ScreenToWorldPoint(raw) :
            raw.normalized;
    }

    public void OnShoot(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        plunger.Shoot(_processedInput);
    }

    public void OnAttack(InputAction.CallbackContext ctx) {
        if (ctx.started) {
            gun.Damaging = true;
        }
        else if (ctx.canceled) {
            gun.Damaging = false;
        }
    }

    public void OnRestart(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private Vector2 _processedInput => playerInput.currentControlScheme == "Keyboard&Mouse"
        ? (_input - (Vector2)transform.position).normalized
        : _input;
}
