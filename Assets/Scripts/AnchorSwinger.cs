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
            new Vector2(_startPosition.x + Mathf.Sin(Time.time * oscillationSpeed) * oscillationDistance,
                rigidBody.position.y)
            );
    }
}
