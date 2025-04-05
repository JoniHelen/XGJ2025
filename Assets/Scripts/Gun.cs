using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Gun : MonoBehaviour {
    [SerializeField] private Color damageColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private float damage;
    [SerializeField] private new Light2D light;
    
    private bool _damaging = false;

    public bool Damaging {
        get => _damaging;
        set {
            _damaging = value;
            light.color = _damaging ? damageColor : normalColor;
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate() {
        if (_damaging) {
            Physics2D.OverlapCircle(transform.position, light.pointLightOuterRadius);
        }
    }
}
