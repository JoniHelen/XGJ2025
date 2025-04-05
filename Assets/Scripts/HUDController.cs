using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour {
    [SerializeField] private TMP_Text depthText;
    [SerializeField] private TMP_Text scoreText;

    public void OnScoreChanged(int score) {
        scoreText.text = score.ToString();
    }

    public void OnDepthChanged(int depth) {
        depthText.text = depth.ToString();
    }

    public void OnLightChanged(float light) {
        
    }
}
