using System;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour {
    [SerializeField] private Elevator elevator;
    [SerializeField] private Gun gun;
    [SerializeField] private TMP_Text depthText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text lightText;
    [SerializeField] private RectTransform bar;

    private void OnEnable() {
        elevator.onDepth.AddListener(OnDepthChanged);
        elevator.onScore.AddListener(OnScoreChanged);
        gun.onLight.AddListener(OnLightChanged);
    }

    private void OnDisable() {
        elevator.onDepth.RemoveListener(OnDepthChanged);
        elevator.onScore.RemoveListener(OnScoreChanged);
        gun.onLight.RemoveListener(OnLightChanged);
    }

    private void OnScoreChanged(int score) {
        scoreText.text = score.ToString();
    }

    private void OnDepthChanged(int depth) {
        depthText.text = depth.ToString();
    }

    private void OnLightChanged(float light) {
        lightText.text = $"{light:000}";
        bar.anchoredPosition = new Vector2((-120 - 243) + 243 * light * 0.01f, bar.anchoredPosition.y);
    }
}
