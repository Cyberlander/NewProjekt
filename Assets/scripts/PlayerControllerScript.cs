using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour 
{
	private Rigidbody2D rb;
	[SerializeField]
	private float speed, maxSpeed;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate()
	{
		float xAxis = Input.GetAxis ("Horizontal");
		float yAxis = Input.GetAxis ("Vertical");

		Vector2 movement = new Vector2 (xAxis, yAxis);

		rb.velocity = movement.normalized * speed;

		transform.LookAt(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f)));

		//if (rb.velocity.magnitude < maxSpeed)
		//	rb.AddForce (movement * speed);
	}

}
