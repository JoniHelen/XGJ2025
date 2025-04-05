using System;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private float health = 100f;
    [SerializeField] private float damageFromLight = 33.33f;
    [SerializeField] private float speed;
    [SerializeField] private float turnRate;
    
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource hurtSource;
    
    [SerializeField] private ParticleSystem hurtParticles;
    [SerializeField] private GameObject deathParticles;
    
    [SerializeField] private Rigidbody2D rb;
    
    private bool _inDamageZone = false;
    public bool InDamageZone {
        get => _inDamageZone;
        set {
            _inDamageZone = value;
            if (_inDamageZone) {
                hurtSource.Play();
                hurtParticles.Play();
            }
            else {
                hurtSource.Stop();
                hurtParticles.Stop();
            }
        }
    }
    
    void Start() {
        
    }

    private void FixedUpdate() {
        if (_inDamageZone) {
            rb.AddForce(((Vector2)transform.position - Elevator.Position).normalized * 
                        (speed  * 2.0f * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }
        else {
            rb.AddForce((Elevator.Position - (Vector2)transform.position).normalized *
                        (speed * Time.fixedDeltaTime), ForceMode2D.Impulse);
        }
    }

    void Update() {
        if (_inDamageZone) {
            TakeDamage(damageFromLight * Time.deltaTime);
        }

        transform.right = (Elevator.Position - (Vector2)transform.position).normalized;
    }

    private void Die() {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void TakeDamage(float damage) {
        health -= damage;

        if (health <= 0) {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent(out Elevator elevator)) {
            elevator.TakeDamage(25.0f);
            rb.linearVelocity = ((Vector2)transform.position - Elevator.Position).normalized * 5.0f;
        }
    }
}
