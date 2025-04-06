using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Rope : MonoBehaviour {
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform[] _points;
    [SerializeField] private GameObject startButton;
    [SerializeField] private ElevatorInputHandler elevatorInputHandler;
    [SerializeField] private Rigidbody2D rb;

    public void Cut() {
        _points = _points.Take(9).ToArray();
    }

    private void Start() {
        StartCoroutine(Descend());
    }

    private IEnumerator Descend() {
        float startHeight = transform.position.y;
        float endHeight = startHeight - 7.5f;

        float duration = 3f;
        float elapsedTime = 0f;
        
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            
            rb.MovePosition(new Vector3(transform.position.x, 
                Mathf.Lerp(startHeight, endHeight, elapsedTime / duration), 
                transform.position.z));
            yield return null;
        }
        
        rb.MovePosition(new Vector3(transform.position.x, endHeight, transform.position.z));
        startButton.SetActive(true);
        elevatorInputHandler.EnableStart();
    }

    void Update() {
        _line.SetPositions(_points.Select(x => x.position).ToArray());
    }
}
