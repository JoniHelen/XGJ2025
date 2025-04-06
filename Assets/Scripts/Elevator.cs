using System.Collections;
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

    [SerializeField] private RectTransform hpBar;

    [SerializeField] private DistanceJoint2D holdingJoint;
    
    [SerializeField] private Rigidbody2D rigidBody2D;
    
    [SerializeField] private UnityEvent onDeath;
    
    public UnityEvent<int> onScore;
    public UnityEvent<int> onDepth;

    private float _depth = 0;
    
    private bool _isDead = false;
    private bool _running = false;
    
    public static Vector2 Position;
    
    void Update() {
        if (_isDead || !_running) return;
        Position = transform.position;
        _depth += Time.deltaTime;
        onDepth.Invoke(Mathf.FloorToInt(_depth));
    }

    public void OnStart() {
        _running = true;
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
        _isDead = true;
    }

    public void TakeDamage(float damage) {
        health -= damage;
        audioSource.PlayOneShot(splinterClip);
        
        hpBar.anchoredPosition = new Vector2(-1.611f + health * 0.01f * 1.611f, hpBar.anchoredPosition.y);
        
        if (health <= 0f) {
            Die();
        }
    }
}
