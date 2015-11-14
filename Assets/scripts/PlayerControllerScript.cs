using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject muzzle;
    [SerializeField]
    private GameObject sparks;
    [SerializeField]
	private float speed;
	[SerializeField]
	private float fireRate; 
	[SerializeField]
	private float flashDuration;
	[SerializeField]
	private AudioClip[] clips;
	[SerializeField]
	private AudioClip shotgun;

	[SerializeField]
	private ObjectPool spawnPool;
	[SerializeField]
	private GameObject enemy1;

    [SerializeField]
    LayerMask canHit = -1;




	private Rigidbody2D rb;
	private Vector3 mousePosition, mouseDirection;
	private bool firing;
	private float lastShotTime;
	private AudioSource aus;
	private AudioSource shotgunAus;


	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		aus = GetComponent<AudioSource> ();
		shotgunAus = muzzle.GetComponent<AudioSource> ();
		firing = false;
		lastShotTime = Time.time;
        sparks = Instantiate(sparks);
	}

	void FixedUpdate()
	{
		mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs (Camera.main.transform.position.z))); 	//Calculates the position of the mouse in WorldSpace (1)
		mouseDirection = Vector3.Normalize(mousePosition - transform.position);																						//Calculates the vector between the mouse and the player object

		Debug.DrawRay (transform.position,mouseDirection, Color.red);																								//debuging

		float xAxis =Input.GetAxis ("Horizontal");
		float yAxis =Input.GetAxis ("Vertical");

		Vector2 movementDir = new Vector2 (xAxis, yAxis);																				


		rb.velocity = movementDir.normalized * speed;                                                                                                               //sets the movement of the player


        transform.rotation = FaceObject(transform.position, mousePosition, FacingDirection.RIGHT);

	}

	void Update()
	{

		if (Input.GetButton ("Fire1") && Time.time > (lastShotTime + fireRate)) {
			shotgunAus.clip = shotgun;
			shotgunAus.Play ();
			StartCoroutine (Fire (mousePosition, mouseDirection));
			lastShotTime = Time.time;
		}
	}


	IEnumerator Fire(Vector3 target, Vector3 targetDir)																												//we nee a coroutine because you can't
	{
		firing = true;																																				//delay in Update
		RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDir, 100, canHit);                                                                                   //this Raycast determits if the player has hit an GameObject
        Debug.DrawLine(transform.position, hit.point, Color.red, 1.5f);
        if (hit.collider != null) 
		{
            if (hit.collider.gameObject.CompareTag("enemy"))
            {                                                                   //is the GameObject an enemy?
                hit.collider.gameObject.GetComponent<Enemy>().Damage(50);
            }
            else if (hit.collider.gameObject.CompareTag("obstacle"))
            {
                SpawnSparks(hit.point);
            }
            else
            {
                Debug.Log("Unexpected target was hit. Expect police investigations. We will take no responsibility.");
            }
		}


        MuzzleFlash();
        yield return new WaitForSeconds(flashDuration);
		firing = false; 
	}


    private void SpawnSparks(Vector2 pos)
    {
        sparks.transform.position = pos;
        sparks.transform.rotation = transform.rotation;
        sparks.GetComponentInChildren<ParticleSystem>().Play();
    }


    private void MuzzleFlash()
    {
        muzzle.GetComponent<Animator>().Play("MuzzleFlash");
    }

	public void Talk()																																						//plays an random audioclip from clips[]
	{
		aus.clip = clips [Random.Range (0, clips.Length)];
		aus.Play ();
	}

	public bool IsTalking()
	{
		return  aus.isPlaying;
	}

     private enum FacingDirection
    {
        UP = 270,
        DOWN = 90,
        LEFT = 180,
        RIGHT = 0
    }

    private static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing)
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

}

// (1) Camera.main.ScreenToWorldSpace converts any screenspace position to an worldspace position using the screenspace position and the distance between the camera and the layer
//		you want to operate in.