using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class MapObject : ScriptableObject
{
    [SerializeField][HideInInspector]
    public List<Vector2> _tilePos;
    [SerializeField][HideInInspector]
    public List<GameObject> _tileType;

    

    void OnEnable()
    {
        if(_tilePos == null)
        {
            _tilePos = new List<Vector2>();
        }
        if (_tileType == null)
        {
            _tileType = new List<GameObject>();
        }
    }

    public bool SetTile(Vector2 slot, GameObject tile)
    {
        if(_tilePos.Contains(slot))
        {
            return false;
        }
       else
        {
            _tilePos.Add(slot);
            _tileType.Add(tile);
            
            return true;
        }
    }

    public void DeleteTile(Vector2 pos)
    {
        if(_tilePos.Contains(pos))
        {
            _tileType.RemoveAt(_tilePos.IndexOf(pos));
            _tilePos.Remove(pos);        
            
        }
    }

}
