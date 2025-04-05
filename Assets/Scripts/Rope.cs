using System.Linq;
using UnityEngine;

public class Rope : MonoBehaviour {
    [SerializeField] private LineRenderer _line;
    [SerializeField] private Transform[] _points;

    public void Cut() {
        _points = _points.Take(9).ToArray();
    }

    void Update() {
        _line.SetPositions(_points.Select(x => x.position).ToArray());
    }
}
