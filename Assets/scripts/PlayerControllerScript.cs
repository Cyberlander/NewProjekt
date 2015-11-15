using UnityEngine;
using System.Collections;

public class PlayerControllerScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject muzzle;
    [SerializeField]
    private float baseSpread;
    [SerializeField]
    private GameObject sparks;
    [SerializeField]
	private float baseSpeed;
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
	private float lastShotTime;
    private float spread;
    private float speed;
	private AudioSource aus;
	private AudioSource shotgunAus;
    private LineRenderer lr;


	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		aus = GetComponent<AudioSource> ();
		shotgunAus = muzzle.GetComponent<AudioSource> ();
        lr = GetComponent<LineRenderer>();
		lastShotTime = Time.time;
        sparks = Instantiate(sparks);
        spread = baseSpread;
        speed = baseSpeed;
	}

	void FixedUpdate()
	{
		mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs (Camera.main.transform.position.z))); 	//Calculates the position of the mouse in WorldSpace (1)
		mouseDirection = Vector3.Normalize(mousePosition - transform.position);																						//Calculates the vector between the mouse and the player object


		float xAxis =Input.GetAxis ("Horizontal");
		float yAxis =Input.GetAxis ("Vertical");

		Vector2 movementDir = new Vector2 (xAxis, yAxis);																				

		rb.velocity = movementDir.normalized * speed;                                                                                                               //sets the movement of the player


        transform.rotation = FaceObject(transform.position, mousePosition, FacingDirection.RIGHT);

	}

	void Update()
	{
        CalculateSpread();
        if(Application.isEditor)
        DrawSpread(spread);

        CalculateAimIndicator();

        if (Input.GetButton("Fire1") && Time.time > (lastShotTime + fireRate))
        {
			shotgunAus.clip = shotgun;
			shotgunAus.Play ();
			Fire (mousePosition, mouseDirection, spread);
			lastShotTime = Time.time;
            spread = baseSpread;
        }
    }

    


    void CalculateAimIndicator()
    {
        Vector2 pos;
        if (Vector2.Distance(transform.position, mousePosition) < 3.5f)
        {
            pos = transform.position + mouseDirection * 3.5f;
        }
        else
        {
            pos = mousePosition;
        }

        float width;
        width = Mathf.Tan(Mathf.Deg2Rad * (spread/2f)) * Vector2.Distance(transform.position, pos) * 4f;
        lr.SetPosition(0, transform.position + (transform.right * 1f));
        lr.SetPosition(1, pos);
        lr.SetWidth(Mathf.Tan(Mathf.Deg2Rad * (spread / 2f)) * 4f, width);
    }


    void CalculateSpread()
    {

        if (Input.GetButton("Fire2"))
        {
            speed = baseSpeed / 2;
            if (spread >= 2f)
                spread = spread - (3.5f * Time.deltaTime);
        }
        else
        {
            spread = baseSpread;
            speed = baseSpeed;
        }
            

    }




    void DrawSpread(float spread)
    {
        if (mouseDirection.y < 0 && mouseDirection.x < 0)
        {
            Debug.DrawRay(transform.position, new Vector2(Mathf.Sin(Mathf.Asin(mouseDirection.x) + Mathf.Deg2Rad * spread),
                                                            -Mathf.Cos(Mathf.Asin(mouseDirection.x) + Mathf.Deg2Rad * spread)),
                                                            Color.blue, Time.deltaTime);

            Debug.DrawRay(transform.position, new Vector2(Mathf.Sin(Mathf.Asin(mouseDirection.x) - Mathf.Deg2Rad * spread),
                                                          -Mathf.Cos(Mathf.Asin(mouseDirection.x) - Mathf.Deg2Rad * (spread))),
                                                            Color.blue, Time.deltaTime);
        }
        else if (mouseDirection.y >= 0)
        {
            Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(Mathf.Acos(mouseDirection.x) + Mathf.Deg2Rad * spread),
                                                            Mathf.Sin(Mathf.Acos(mouseDirection.x) + Mathf.Deg2Rad * spread)),
                                                            Color.blue, Time.deltaTime);

            Debug.DrawRay(transform.position, new Vector2(Mathf.Cos(Mathf.Acos(mouseDirection.x) - Mathf.Deg2Rad * spread),
                                                            Mathf.Sin(Mathf.Acos(mouseDirection.x) - Mathf.Deg2Rad * spread)),
                                                            Color.blue, Time.deltaTime);
        }

        else
        {
            Debug.DrawRay(transform.position, new Vector2(Mathf.Sin(Mathf.Acos(mouseDirection.y) + Mathf.Deg2Rad * spread),
                                                            Mathf.Cos(Mathf.Acos(mouseDirection.y) + Mathf.Deg2Rad * spread)),
                                                            Color.blue, Time.deltaTime);

            Debug.DrawRay(transform.position, new Vector2(Mathf.Sin(Mathf.Acos(mouseDirection.y) - Mathf.Deg2Rad * spread),
                                                            Mathf.Cos(Mathf.Acos(mouseDirection.y) - Mathf.Deg2Rad * spread)),
                                                            Color.blue, Time.deltaTime);
        }
    }

    Vector2 CalculateSpreadVector(Vector2 lead,float spread)
    {
        if (lead.y < 0 && lead.x < 0)
        {
            return new Vector2(Random.Range(Mathf.Sin(Mathf.Asin(lead.x) + Mathf.Deg2Rad * spread),
                                            Mathf.Sin(Mathf.Asin(lead.x) - Mathf.Deg2Rad * spread)),
                               Random.Range(-Mathf.Cos(Mathf.Asin(lead.x) + Mathf.Deg2Rad * spread),
                                            -Mathf.Cos(Mathf.Asin(lead.x) - Mathf.Deg2Rad * spread)));
        }
        else if (lead.y >= 0)
        {
            return new Vector2(Random.Range(Mathf.Cos(Mathf.Acos(mouseDirection.x) + Mathf.Deg2Rad * spread),
                                            Mathf.Cos(Mathf.Acos(mouseDirection.x) - Mathf.Deg2Rad * spread)),
                               Random.Range(Mathf.Sin(Mathf.Acos(mouseDirection.x) + Mathf.Deg2Rad * spread),
                                            Mathf.Sin(Mathf.Acos(mouseDirection.x) - Mathf.Deg2Rad * spread)));


        }

        else
        {
            return new Vector2(Random.Range(Mathf.Sin(Mathf.Acos(mouseDirection.y) + Mathf.Deg2Rad * spread),
                                            Mathf.Sin(Mathf.Acos(mouseDirection.y) - Mathf.Deg2Rad * spread)),
                               Random.Range(Mathf.Cos(Mathf.Acos(mouseDirection.y) + Mathf.Deg2Rad * spread),
                                            Mathf.Cos(Mathf.Acos(mouseDirection.y) - Mathf.Deg2Rad * spread)));
        }
    }


	void Fire(Vector3 target, Vector3 targetDir, float spread)																												//we nee a coroutine because you can't
	{																																			//delay in Update
		RaycastHit2D hit = Physics2D.Raycast(transform.position, CalculateSpreadVector(targetDir, spread), 100, canHit);                                                                                   //this Raycast determits if the player has hit an GameObject
        Debug.DrawLine(transform.position, hit.point, Color.red, 1.5f);
        if (hit.collider != null) 
		{
            if (hit.collider.gameObject.CompareTag("enemy"))
            {                                                                   //is the GameObject an enemy?
                hit.collider.gameObject.GetComponent<Enemy>().Damage(50, hit.point);
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