using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour 
{	
	private const float MAP_WIDTH = 30;
	private const float MAP_HEIGHT = 20;


	[SerializeField]
	private GameObject player;

	private float zPos;


	void Start()
	{
		zPos = transform.position.z;
	}

	void Update () 
	{
		if ((Camera.main.ViewportToWorldPoint (new Vector3(0, 0.5f, Mathf.Abs (Camera.main.transform.position.z))).x < -MAP_WIDTH / 2))
		{
			gameObject.transform.position = new Vector3(gameObject.transform.position.x + 0.1f,gameObject.transform.position.y, gameObject.transform.position.z );
		}

		transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Vector2.Distance (transform.position, player.transform.position)/100);
		transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
	}
}
