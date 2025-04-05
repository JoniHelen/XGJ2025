using System.Collections;
using UnityEngine;

public class Plunger : MonoBehaviour {
    [SerializeField] private float speed;
    [SerializeField] private float recallTime;
    [SerializeField] private float recallRadius;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform gun;
    [SerializeField] private Elevator elevator;

    [SerializeField] private AudioClip shotClip;
    [SerializeField] private AudioClip hitClip;
    
    [SerializeField] private AudioSource audioSource;
    
    private Rigidbody2D _rigidbody;
    private readonly Vector3[] _points = new Vector3[2];
    private bool _loaded = true;
    private bool _recalling = false;
    private Antiquity _antiquity;

    private void Awake() {
        TryGetComponent(out _rigidbody);
    }

    private void Update() {
        _points[0] = transform.TransformPoint(Vector3.down * 0.5f);
        _points[1] = gun.position;
        line.SetPositions(_points);

        if (_rigidbody.simulated) {
            transform.up = _rigidbody.linearVelocity;
        }

        if ((transform.position - gun.position).magnitude > recallRadius && !_recalling) {
            Recall();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.TryGetComponent(out Antiquity antiquity)) {
            antiquity.transform.parent = transform;
            _antiquity = antiquity;
        }
        
        audioSource.PlayOneShot(hitClip);
        
        Recall();
    }

    public void Shoot(Vector2 direction) {
        if (!_loaded) return;
        
        audioSource.PlayOneShot(shotClip);
        
        transform.parent = null;
        _rigidbody.simulated = true;
        _rigidbody.linearVelocity = direction * speed;
        _loaded = false;
    }

    private void Recall() {
        _rigidbody.simulated = false;
        _rigidbody.angularVelocity = 0;
        _recalling = true;
        StartCoroutine(RecallCoroutine(recallTime));
    }

    private IEnumerator RecallCoroutine(float seconds) {
        Vector2 start = transform.position;
        Vector2 end = gun.position;

        var t = 0f;
        
        while (t < seconds) {
            transform.position = Vector3.Lerp(start, end, t / seconds);
            yield return null;
            t += Time.deltaTime;
        }
        
        transform.parent = gun;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        _loaded = true;
        _recalling = false;

        if (!_antiquity) yield break;
        
        elevator.AddScore(_antiquity.Value);
        Destroy(_antiquity.gameObject);
        _antiquity = null;
    }
}
