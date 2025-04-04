using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class ElevatorInputHandler : MonoBehaviour {
    [SerializeField] private Transform gun;
    [SerializeField] private Plunger plunger;
    
    private Vector2 _input;
    private Camera _camera;
    
    void Start() {
        _camera = Camera.main;
    }

    void Update() {
        gun.up = _input;
    }

    public void OnLook(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        
        var raw = ctx.ReadValue<Vector2>();
        
        if (raw.sqrMagnitude < 0.3f) return; // Stick dead zone
        
        _input = ctx.control.device.deviceId == Mouse.current.deviceId ? 
            ((Vector2)_camera.ScreenToWorldPoint(raw) - (Vector2)transform.position).normalized :
            raw.normalized;
    }

    public void OnShoot(InputAction.CallbackContext ctx) {
        if (!ctx.performed) return;
        plunger.Shoot(_input);
    }
}
