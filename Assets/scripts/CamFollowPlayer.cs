using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour 
{	
	
	private GameObject _player;
	private Rigidbody2D _rb;

	void Start()
	{
        _player = GameObject.FindGameObjectWithTag("Player");
		_rb = GetComponent<Rigidbody2D> ();
	}

	void Update () 
	{
		_rb.velocity =  (_player.transform.position - transform.position);
	}
}
