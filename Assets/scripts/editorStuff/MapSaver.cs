using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapSaver : MonoBehaviour {

    public GameObject _map;
    public string _path;
	
	// Update is called once per frame
	public void Save ()
    {
       
            GameObject pref = PrefabUtility.CreatePrefab("Assets/" + _path + ".prefab", _map, ReplacePrefabOptions.ReplaceNameBased);
            pref = Instantiate(pref);
            DestroyImmediate(pref.GetComponent<MapBuilder>());
            PrefabUtility.CreatePrefab("Assets/" + _path + ".prefab", pref, ReplacePrefabOptions.ReplaceNameBased);
            Destroy(pref);
       
	}

    public void SetPath(string path)
    {
        _path = path;
    }
}
