using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject muzzle;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float fireRate; 
	[SerializeField]
	private float flashDuration;

	private LineRenderer lr;
	private Rigidbody2D rb;
	private Vector3 mousePosition, mouseDirection;
	private bool firing;
	private float lastShotTime;


	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		lr = GetComponent<LineRenderer> ();
		firing = false;
		lastShotTime = Time.time;
	}

	void FixedUpdate()
	{
		mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs (Camera.main.transform.position.z))); 	//Calculates the position of the mouse in WorldSpace (1)
		mouseDirection = Vector3.Normalize(mousePosition - transform.position);												//Calculates the vector between the mouse and the player object

		Debug.DrawRay (transform.position,mouseDirection, Color.red);																//debuging

		float xAxis =Input.GetAxis ("Horizontal");
		float yAxis =Input.GetAxis ("Vertical");

		Vector2 movementDir = new Vector2 (xAxis, yAxis);																				


		rb.velocity = movementDir.normalized * speed;																				//sets the movement of the player


		RotateToDirection (mouseDirection);

	}

	void Update()
	{

		if (Input.GetButton("Fire1") && Time.time > (lastShotTime +  fireRate)) 																																					
		{
			StartCoroutine (Fire (mousePosition, mouseDirection));
			lastShotTime = Time.time;
		}
	}


	IEnumerator Fire(Vector3 target, Vector3 targetDir)
	{
		firing = true;
		RaycastHit2D hit = Physics2D.Raycast(muzzle.transform.position, targetDir);

		if (hit.collider.gameObject.tag.Equals("enemy"))
		{
			hit.collider.gameObject.GetComponent<Enemy>().Die();
		}


		lr.enabled = true;
		lr.SetPosition (0, muzzle.transform.position);
		lr.SetPosition (1, target);
		yield return new WaitForSeconds(flashDuration);
		lr.enabled = false;
		firing = false; 
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