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

//the camera moves towards the 2d plane utill the corner of the plane aligns with the left edge of the viewport. this ensures the plane fills the viewport