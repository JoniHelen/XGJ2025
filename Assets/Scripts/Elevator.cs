using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Elevator : MonoBehaviour {
    [SerializeField] private float health = 100f;
    [SerializeField] private int score = 0;
    
    [SerializeField] private float deathSpeed = 1.5f;
    [SerializeField] private Rope rope;
    
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip splinterClip;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private DistanceJoint2D holdingJoint;
    
    [SerializeField] private Rigidbody2D rigidBody2D;
    
    [SerializeField] private UnityEvent onDeath;
    
    [SerializeField] private UnityEvent<int> onScore;
    [SerializeField] private UnityEvent<int> onDepth;

    private float _depth = 0;
    
    public static Vector2 Position;
    
    void Update() {
        Position = transform.position;
        _depth += Time.deltaTime;
        onDepth.Invoke(Mathf.FloorToInt(_depth));
    }

    public void AddScore(int scoreToAdd) {
        score += scoreToAdd;
        audioSource.PlayOneShot(coinClip);
        onScore.Invoke(score);
    }

    private void Die() {
        rope.Cut();
        holdingJoint.connectedBody = null;
        rigidBody2D.linearVelocity = 
            Quaternion.AngleAxis(Random.Range(-45f, 45f), Vector3.forward) * Vector2.up * deathSpeed;
        rigidBody2D.angularVelocity = Random.Range(-45f, 45f);
        onDeath.Invoke();
    }

    public void TakeDamage(float damage) {
        health -= damage;
        audioSource.PlayOneShot(splinterClip);
        if (health <= 0f) {
            Die();
        }
    }
}
