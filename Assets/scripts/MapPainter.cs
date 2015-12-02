using UnityEngine;
using System.Collections;

public class MapPainter : MonoBehaviour
{
    public GameObject _currentTile;

    public MapObject _map;

    public MapBuilder _mB;

    public int _gridsize = 4;

    private Vector3 _mousePosition;
    private int x, y;

    void Update ()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.y)));
        x = Mathf.RoundToInt(_mousePosition.x / _gridsize) * _gridsize;
        y = Mathf.RoundToInt(_mousePosition.z / _gridsize) * _gridsize;
        if (Input.GetMouseButton(0))
        {
            _map.SetTile(new Vector2(x, y), _currentTile);
            _mB.Build();
        }
        if (Input.GetMouseButton(1))
        {
            _map.DeleteTile(new Vector2(x, y));
            _mB.Build();
        }

    }
}
