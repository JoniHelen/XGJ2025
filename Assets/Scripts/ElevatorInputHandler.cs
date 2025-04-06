using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ElevatorInputHandler : MonoBehaviour {
    [SerializeField] private Gun gun;
    [SerializeField] private Plunger plunger;
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private GameObject guide;
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject hud;
    
    [SerializeField] private UnityEvent onStart;
    
    private Vector2 _input;
    private Camera _camera;
    
    void Awake() {
        _camera = Camera.main;
    }

    void Update() {
        gun.transform.up = _processedInput;

        if (Keyboard.current != null) {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) {
                Application.Quit();
            }
        }

        if (Gamepad.current != null) {
            if (Gamepad.current.selectButton.wasPressedThisFrame) {
                Application.Quit();
            }
        }
    }

    public void OnPlayerDeath() {
        playerInput.SwitchCurrentActionMap("Dead");
    }

    public void ButtonStart() {
        onStart.Invoke();
        start.SetActive(false);
        hud.SetActive(true);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void OnStart(InputAction.CallbackContext context) {
        if (!context.performed) return;
        onStart.Invoke();
        start.SetActive(false);
        hud.SetActive(true);
        playerInput.SwitchCurrentActionMap("Player");
    }

    public void OnLook(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        
        var raw = ctx.ReadValue<Vector2>();
        
        if (raw.sqrMagnitude < 0.3f) return; // Stick dead zone
        
        _input = ctx.control.device.deviceId == Mouse.current.deviceId ? 
            _camera.ScreenToWorldPoint(raw) :
            raw.normalized;
    }

    public void OnInfo(InputAction.CallbackContext context) {
        if(!context.performed) return;
        
        if (guide.activeSelf) {
            guide.SetActive(false);
            start.SetActive(true);
        }
        else if (start.activeSelf) {
            guide.SetActive(true);
            start.SetActive(false);
        }
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
