using System;
using UnityEngine;

public class AnchorSwinger : MonoBehaviour {
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float oscillationDistance;
    [SerializeField] private float oscillationSpeed;

    private Vector2 _startPosition;
    
    void Start() {
        _startPosition = rigidBody.position;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        rigidBody.MovePosition(
            _startPosition + Vector2.right * (Mathf.Sin(Time.time * oscillationSpeed) * oscillationDistance)
        );
    }
}
