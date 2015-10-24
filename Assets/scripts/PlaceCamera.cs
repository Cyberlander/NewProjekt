using UnityEngine;
using System.Collections;

public class PlaceCamera : MonoBehaviour 
{	
	public Transform corner;
	void Start () 
	{
		while (Camera.main.WorldToViewportPoint (corner.position).x > 0)
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + 0.02f);
	}
}
