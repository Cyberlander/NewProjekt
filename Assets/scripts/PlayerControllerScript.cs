using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour 
{
	private Rigidbody2D rb;
	[SerializeField]
	private float speed;
	private LineRenderer lr;
	[SerializeField]
	private GameObject muzzle;


	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		lr = GetComponent<LineRenderer> ();
	}

	void FixedUpdate()
	{
		Vector3 mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs (Camera.main.transform.position.z))); 	//Calculates the position of the mouse in WorldSpace (1)
		Vector3 mouseDirection = Vector3.Normalize(mousePosition - transform.position);												//Calculates the vector between the mouse and the player object

		Debug.DrawRay (transform.position,mouseDirection, Color.red);																//debuging

		float xAxis =Input.GetAxis ("Horizontal");
		float yAxis =Input.GetAxis ("Vertical");

		Vector2 movementDir = new Vector2 (xAxis, yAxis);																				


		rb.velocity = movementDir.normalized * speed;																				//sets the movement of the player


		RotateToDirection (mouseDirection);
		if (Input.GetButtonDown ("Fire1"))
			Fire (mousePosition, mouseDirection);


	}

	private void Fire(Vector3 target, Vector3 targetDir)
	{
		RaycastHit2D hit = Physics2D.Raycast(muzzle.transform.position, targetDir);

		if (hit.collider.gameObject.tag.Equals("enemy"))
		{
			hit.collider.gameObject.SetActive (false);
		}
		

		lr.SetPosition (0, muzzle.transform.position);
		lr.SetPosition (1, target);

	}

	private void RotateToDirection(Vector3 mouseDirection)
	{
		Quaternion rotation = Quaternion.LookRotation
			(mouseDirection, transform.TransformDirection(Vector3.up));
		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
		
		
		if (mouseDirection != transform.right) 
		{
			rb.MoveRotation(180);
		}
		return;
	}

}

// (1) Camera.main.ScreenToWorldSpace converts any screenspace position to an worldspace position using the screenspace position and the distance between the camera and the layer
//		you want to operate in.