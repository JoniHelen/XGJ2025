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

        var aIncrement = 60.0f / a;
        var bIncrement = 60.0f / b;

        var angleA = -30.0f;
        var angleB = -30.0f;
        
        for (int i = 1; i <= a; i++) {
            angleA += Random.Range(aIncrement / 2.0f, aIncrement);
            var hit = Physics2D.Raycast(
                wall.transform.position,
                Quaternion.AngleAxis(angleA, Vector3.forward) * Vector2.right);
            
            if (hit) {
                if (hit.collider.gameObject.TryGetComponent(out Antiquity _)) {
                    continue;
                }
                
                Instantiate(antiquities[0], hit.point,
                    Quaternion.AngleAxis(Vector2.Angle(hit.normal, Vector2.right), Vector3.forward),
                    wall.transform);
            }
            
            angleA = i * aIncrement - 30.0f;
        }
        
        for (int i = 1; i <= b; i++) {
            angleB += Random.Range(bIncrement / 2.0f, bIncrement);
            var hit = Physics2D.Raycast(
                wall.transform.position,
                Quaternion.AngleAxis(angleB, Vector3.forward) * Vector2.left);
            
            if (hit) {
                if (hit.collider.gameObject.TryGetComponent(out Antiquity _)) {
                    continue;
                }
                
                Instantiate(antiquities[0], hit.point,
                    Quaternion.AngleAxis(Vector2.Angle(hit.normal, Vector2.right), Vector3.forward),
                    wall.transform);
            }
            
            angleB = i * bIncrement - 30.0f;
        }
        
        wall.position = (Vector2)_wallQueue.ElementAt(1).position + Vector2.down * 10.0f;
    }
}
