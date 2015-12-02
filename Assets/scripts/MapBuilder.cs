using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MapBuilder : MonoBehaviour
{
    public MapObject _map;

    private Dictionary<Vector2, GameObject> _buildTiles;

    void Start()
    {
        
        _buildTiles = new Dictionary<Vector2, GameObject>();
        Build();
       
    }

    public void  Build()
    {
        Clear();
        foreach (Vector2 slot in _map._tilePos)
        {
            if (!_buildTiles.ContainsKey(slot))
            {
                GameObject tile = _map._tileType[_map._tilePos.IndexOf(slot)];
                Vector3 s = new Vector3(slot.x, 0, slot.y);
                GameObject g = Instantiate(tile, s, Quaternion.identity) as GameObject;
                g.transform.parent = transform;
                _buildTiles.Add(slot, g);
            }

           
        }
    }

    private void Clear()
    {
        Vector2[] keys = new Vector2[_buildTiles.Keys.Count];
        _buildTiles.Keys.CopyTo(keys, 0);
       foreach (Vector2 v in keys)
        {
           if(!_map._tilePos.Contains(v))
            {
                GameObject g;
                _buildTiles.TryGetValue(v, out g);
                Destroy(g);
                _buildTiles.Remove(v);
            }


        }
    }

    private void KillAllChildren()
    {
        Transform[] children =  transform.GetComponentsInChildren<Transform>();

        foreach (Transform t in children)
        {
            if(t.gameObject != gameObject)
                DestroyImmediate(t.gameObject);
        }
    }
}
