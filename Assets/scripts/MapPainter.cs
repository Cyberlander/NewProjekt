using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MapPainter : MonoBehaviour
{
    public GameObject _currentTile;

    public MapObject _map;

    public MapBuilder _mB;

    public int _gridsize = 4;

    public Mesh _mesh;


    List<Object> _tileSet;
    private Vector3 _mousePosition;
    private int x, y;

    void Start()
    {
        _tileSet = new List<Object>() ;
        _tileSet.AddRange((Object[]) Resources.LoadAll("tiles/prefabs", typeof(GameObject)));      
        SetCurrentTile("grass");
    }

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

    public void SetCurrentTile(string tileName)
    {
        foreach (GameObject t in _tileSet)
        {
            if(t.name == tileName)
            {                   
                _currentTile = t; 
                return;
            }
        }
        GameObject gObj = new GameObject(tileName);
        MeshFilter mf = gObj.AddComponent<MeshFilter>();
        mf.mesh = _mesh;
        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.mainTexture = (Texture) Resources.Load("tiles/" + tileName);
        AssetDatabase.CreateAsset(mat, "Assets/materials/BGMaterials/" + tileName + ".asset");
        MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
        mr.material = mat; 
        _tileSet.Add(PrefabUtility.CreatePrefab("Assets/Resources/tiles/prefabs/" + tileName + ".prefab", gObj, ReplacePrefabOptions.ReplaceNameBased));
        AssetDatabase.SaveAssets();
        Destroy(gObj);
        SetCurrentTile(tileName);

    }
}
