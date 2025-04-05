using System.Linq;
using UnityEngine;

public class Rope : MonoBehaviour {
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform[] _points;
    
    /*// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }*/

    void Update() {
        _line.SetPositions(_points.Select(x => x.position).ToArray());
    }
}
