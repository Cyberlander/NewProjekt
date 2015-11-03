using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour 
{	
	private List<GameObject> spawned;
	private List<GameObject> stash;




	public GameObject Spawn (Vector3 pos, GameObject type)
	{
		GameObject o = stash.Find (e => e.GetComponent<Enemy> ().GetType () == type.GetComponent<Enemy> ().GetType ());

		
		if (o == null) 
		{
			GameObject fresh = Instantiate (type);
			fresh.GetComponent<Enemy>().SetObjectPool(this);
			stash.Add(fresh);
			fresh.transform.parent = gameObject.transform;
			o =fresh;
		} 

		spawned.Add (o);
		stash.Remove(o);
		o.SetActive (gameObject);
		o.transform.position = pos;
		return  o;
	}


	public void Despawn(GameObject obj)
	{
		if (obj == null) 
		{
			Debug.Log ("ObjectPool: ERROR! No Object to Despawn");
			return;
		}
		spawned.Remove (obj);
		stash.Add (obj);
		obj.transform.parent = gameObject.transform;
		obj.SetActive (false);
	}

	public int GetSpawnedObjectCount()
	{
		return spawned.Count;
	}

	void Start()
	{
		spawned = new List<GameObject>();
		stash = new List<GameObject>();
	}


}
