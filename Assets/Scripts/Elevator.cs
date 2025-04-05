using UnityEngine;

public class Elevator : MonoBehaviour {
    
    [SerializeField] private float health = 100f;
    [SerializeField] private int score = 0;
    
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScore(int scoreToAdd) {
        score += scoreToAdd;
        audioSource.PlayOneShot(coinClip);
    }
}
