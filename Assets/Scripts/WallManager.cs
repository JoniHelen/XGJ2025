using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallManager : MonoBehaviour {
    [SerializeField] private float scrollSpeed;
    [SerializeField] private float verticalLimit;
    [SerializeField] private int generatedTreasures;
    [SerializeField] private List<Transform> walls;
    [SerializeField] private List<Antiquity> antiquities;
    [SerializeField] private ContactFilter2D contactFilter;
    
    private Queue<Transform> _wallQueue;
    
    void Start() {
        _wallQueue = new Queue<Transform>(walls);
    }

    void FixedUpdate() {
        foreach (var tr in walls) {
            tr.position = (Vector2)tr.position + Vector2.up * (scrollSpeed * Time.deltaTime);
        }

        if (_wallQueue.Peek().position.y >= verticalLimit) {
            Generate(_wallQueue.Dequeue());
        }
    }

    public void OnPlayerDeath() {
        enabled = false;
    }

    private void Generate(Transform wall) {
        _wallQueue.Enqueue(wall);

        foreach (var antiquity in wall.GetComponentsInChildren<Antiquity>()) {
            Destroy(antiquity.gameObject);
        }

        var a = generatedTreasures / 2;
        var b = generatedTreasures % 2 == 0 ? generatedTreasures / 2 : generatedTreasures % 2;
        
        for (int i = 0; i < a; i++) {
            var angle = Random.Range(0.0f, 60.0f) - 30.0f;
            var hit = Physics2D.Raycast(
                wall.transform.position,
                Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.right);

            if (hit) {
                Instantiate(antiquities[0], hit.point,
                    Quaternion.AngleAxis(Vector2.Angle(hit.normal, Vector2.right), Vector3.forward),
                    wall.transform);
            }
        }
        
        for (int i = 0; i < b; i++) {
            var angle = Random.Range(0.0f, 60.0f) - 30.0f;
            var hit = Physics2D.Raycast(
                wall.transform.position,
                Quaternion.AngleAxis(angle, Vector3.forward) * Vector2.left);

            if (hit.collider.gameObject.TryGetComponent(out Antiquity _)) {
                continue;
            }

            if (hit) {
                Instantiate(antiquities[0], hit.point,
                    Quaternion.AngleAxis(Vector2.Angle(hit.normal, Vector2.right), Vector3.forward),
                    wall.transform);
            }
        }
        
        wall.position = (Vector2)_wallQueue.ElementAt(1).position + Vector2.down * 10.0f;
    }
}
