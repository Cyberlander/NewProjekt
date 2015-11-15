using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class ObjectPool : MonoBehaviour 
{	
	private List<GameObject> _spawned;
	private List<GameObject> _stash;

    [SerializeField]
    private GameObject[] _fillTypes;

    [SerializeField]
    private int[] _fillCount;

	public GameObject Spawn (Vector3 pos, GameObject type)
	{
		GameObject o = _stash.Find (e => e.GetComponent<ObjectPoolable>().GetType() == type.GetComponent<ObjectPoolable>().GetType());
        
		
		if (o == null) 
		{
			o = CreateNewElement(type);
		} 

		_spawned.Add (o);
		_stash.Remove(o);
		o.SetActive (gameObject);
		o.transform.position = pos;
		return  o;
	}

    private GameObject CreateNewElement(GameObject type)
    {
        GameObject fresh = Instantiate(type);
        if(fresh.GetComponent<ObjectPoolable>() != null)
            fresh.GetComponent<ObjectPoolable>().SetObjectPool(this);
        _stash.Add(fresh);
        fresh.transform.parent = gameObject.transform;
        fresh.SetActive(false);
        return fresh;
    }


    private void FillObjectPool(GameObject[] types, int[] count)
    {
        for(int i = 0; i < types.Length; i++)
        {
            for(int n = 1; n <= count[i]; n++)
            {
                CreateNewElement(types[i]);
            }
        }
    }
    
	public void Despawn(GameObject obj)
	{
		if (obj == null) 
		{
			Debug.Log ("ObjectPool: ERROR! No Object to Despawn");
			return;
		}
		_spawned.Remove (obj);
		_stash.Add (obj);
		obj.transform.parent = gameObject.transform;
		obj.SetActive (false);
	}

	public int GetSpawnedObjectCount()
	{
		return _spawned.Count;
	}

    public int GetSpawnedEnemyCount()
    {
        int count = 0;
        foreach (GameObject o in _spawned)
        {
            if (o.GetComponent<Enemy>() != null)
                count++;
        }
        return count;
    }

	void Start()
	{
		_spawned = new List<GameObject>();
		_stash = new List<GameObject>();
        this.FillObjectPool(_fillTypes, _fillCount);
	}
}
