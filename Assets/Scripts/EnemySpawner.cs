using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private float spawnInterval;
    [SerializeField] private GameObject enemyPrefab;
    
    private WaitForSeconds _spawnDelay;
    
    void Start() {
        _spawnDelay = new WaitForSeconds(spawnInterval);
        StartCoroutine(Spawner());
    }

    public void OnPlayerDeath() {
        StopAllCoroutines();
    }

    private IEnumerator Spawner() {
        while (true) {
            yield return _spawnDelay;
        
            float yPosition = Random.value < 0.5f ? 6.0f : -6.0f;
            float xPosition = Random.value - 0.5f * 8.0f;
        
            Instantiate(enemyPrefab, new Vector3(xPosition, yPosition), Quaternion.identity);
        }
    }
}
