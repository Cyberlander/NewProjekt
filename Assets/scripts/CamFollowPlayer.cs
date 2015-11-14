using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour 
{	
	private const float MAP_WIDTH = 30;
	private const float MAP_HEIGHT = 20;


	[SerializeField]
	private GameObject player;
	private Rigidbody2D rb;


	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		transform.localScale = new Vector3 (Camera.main.aspect, 1, 1); 
	}

	void Update () 
	{
		rb.velocity =  (player.transform.position - transform.position);
	}
}
