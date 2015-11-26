using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class fenceBuilder : MonoBehaviour
{  
    [Range(1,100)]
    public int _length = 1;
    public GameObject _fenceObject;  
     
    [SerializeField][HideInInspector]
    private int _currentLength = 0;   

    [SerializeField][HideInInspector]
    private List<GameObject> _fenceParts = new List<GameObject>();
    
		
	// Update is called once per frame
	void Update ()
    {        
        if (_currentLength <= 0)
        {
            GameObject o = (GameObject)Instantiate(_fenceObject, transform.position, transform.rotation);
            _fenceParts.Add(o);
            o.transform.parent = gameObject.transform;
            _currentLength = 1;                    
            if (_length <= 1)
            {
                _length = 1;
            }
        }
        while (_currentLength < _length)
        {
            GameObject o = (GameObject)Instantiate(_fenceObject, _fenceParts[_currentLength - 1].transform.position + 2 * transform.forward, transform.rotation);
           _fenceParts.Add(o);
            _currentLength++;
            o.transform.parent = gameObject.transform;                
        }
        while((_currentLength > _length))
        {
            GameObject o = _fenceParts[_currentLength - 1];
            _fenceParts.Remove(o);
            _currentLength--;
            DestroyImmediate(o);                      
        }       

    }
}
