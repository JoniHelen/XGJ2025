using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;
using UnityEngine.Events;

public class Gun : MonoBehaviour {
    [SerializeField] private Color damageColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private float damage;
    [SerializeField] private float lightDrain;
    [SerializeField] private new Light2D light;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource switchSource;
    [SerializeField] private AudioClip switchClip;
    [SerializeField] private AudioClip onClip;
    [SerializeField] private AudioClip offClip;
    [SerializeField] private ContactFilter2D contactFilter;

    [SerializeField] private UnityEvent<float> onLight;
    
    private bool _damaging = false;

    private float _lightResource = 100.0f;

    private readonly List<Enemy> _enemies = new();

    public bool Damaging;

    private bool ActuallyDamaging {
        get => _damaging;
        set {
            if (_damaging == value) return;
            
            _damaging = value;
            switchSource.PlayOneShot(switchClip);
            light.color = _damaging ? damageColor : normalColor;
            audioSource.Stop();
            audioSource.PlayOneShot(_damaging ? onClip : offClip);
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update() {
        _lightResource = Damaging ? 
            Mathf.Max(_lightResource - lightDrain * Time.deltaTime, 0.0f) : 
            Mathf.Min(_lightResource + lightDrain * 0.5f * Time.deltaTime, 100.0f);

        onLight.Invoke(_lightResource);
        
        ActuallyDamaging = _lightResource > 0.0f && Damaging;
    }

    void FixedUpdate() {
        if (_damaging && _lightResource > 0) {
            var results = new List<Collider2D>();
            var count = Physics2D.OverlapCircle(
                transform.position, light.pointLightOuterRadius, contactFilter, results
            );

            var detectedEnemies = results.Select(result => result.GetComponent<Enemy>());

            if (count > 0) {
                foreach (var enemy in detectedEnemies) {
                    if (_enemies.Contains(enemy)) continue;
                    
                    if (Vector2.Angle(transform.up,
                            enemy.transform.position - transform.position) > light.pointLightOuterAngle / 2.0f) {
                        continue;
                    }
                    
                    enemy.InDamageZone = true;
                    _enemies.Add(enemy);
                }
            }

            _enemies.RemoveAll(x => !x);

            foreach (var enemy in _enemies) {
                if (Vector2.Angle(transform.up,
                        enemy.transform.position - transform.position) > light.pointLightOuterAngle / 2.0f) {
                    enemy.InDamageZone = false;
                    continue;
                }

                if (!detectedEnemies.Contains(enemy)) {
                    enemy.InDamageZone = false;
                }
            }

            _enemies.RemoveAll(enemy => !enemy.InDamageZone);
        }
        else if (_enemies.Count > 0) {
            foreach (var enemy in _enemies) {
                enemy.InDamageZone = false;
            }
            
            _enemies.Clear();
        }
    }
}
