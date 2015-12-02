using UnityEngine;
using System.Collections;


public class e_cameraController : MonoBehaviour {

    public float _scrollSpeed, _moveSpeed;

    private Vector3 _mouseButtonDown;
    private Vector3 _mousePosition;

    void Update()
    {
        
        _mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(Camera.main.transform.position.y)));
        

        if (Input.GetMouseButtonDown(2))
        {
            _mouseButtonDown = _mousePosition;
            
        }
        else if (Input.GetMouseButton(2))
        {
            transform.position += new Vector3((_mouseButtonDown.x - _mousePosition.x), 0, (_mouseButtonDown.z - _mousePosition.z));
        }




        gameObject.transform.position += new Vector3(0, -(Input.GetAxis("Mouse ScrollWheel")) * _scrollSpeed, 0);
        gameObject.transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, 0.5f, 1000), transform.position.z);
    }
}
