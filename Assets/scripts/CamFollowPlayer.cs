using UnityEngine;
using System.Collections;

public class CamFollowPlayer : MonoBehaviour 
{	
	
	private GameObject _player;
	private Rigidbody _rb;

	void Start()
	{
        _player = GameObject.FindGameObjectWithTag("Player");
		_rb = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		_rb.velocity =  _player.transform.position - transform.position;
	}
}
