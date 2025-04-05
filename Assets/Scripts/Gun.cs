using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Linq;

public class Gun : MonoBehaviour {
    [SerializeField] private Color damageColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private float damage;
    [SerializeField] private new Light2D light;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource switchSource;
    [SerializeField] private AudioClip switchClip;
    [SerializeField] private AudioClip onClip;
    [SerializeField] private AudioClip offClip;
    [SerializeField] private ContactFilter2D contactFilter;
    
    private bool _damaging = false;

    private readonly List<Enemy> _enemies = new();

    public bool Damaging {
        get => _damaging;
        set {
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

    void FixedUpdate() {
        if (_damaging) {
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
