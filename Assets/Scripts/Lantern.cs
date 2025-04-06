using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Lantern : MonoBehaviour {
    [SerializeField] private float flickerSpeed;
    [SerializeField] private Light2D lanternLight;

    void Update() {
        lanternLight.intensity = (Mathf.Sin(Time.time * flickerSpeed) + 1.0f) * 0.25f + 0.5f;
    }
}

