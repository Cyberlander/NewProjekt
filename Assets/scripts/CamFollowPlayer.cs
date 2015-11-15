using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour 
{	
	[SerializeField]
	private GameObject player;
	private Rigidbody2D rb;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void Update () 
	{
		rb.velocity =  (player.transform.position - transform.position);
	}
}
