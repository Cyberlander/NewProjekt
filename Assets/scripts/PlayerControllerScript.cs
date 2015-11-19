using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControllerScript : MonoBehaviour 
{
	[SerializeField]
	private GameObject _muzzle;
    [SerializeField]
    private GameObject _impact;
    [SerializeField]
	private float _baseSpeed;	
	[SerializeField]
	private AudioClip[] clips;

    public WeaponObject _weapon;
    

	[SerializeField]
	private ObjectPool spawnPool;

    [SerializeField]
    LayerMask canHit = -1;




	private Rigidbody rb;
	private Vector3 mousePosition, mouseDirection;
	private float lastShotTime;
    private bool _reloading;
    private float spread;
    private float speed;
	private AudioSource aus;
	private AudioSource shotgunAus;
    private LineRenderer lr;


	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		aus = GetComponent<AudioSource> ();
		shotgunAus = _muzzle.GetComponent<AudioSource> ();
        lr = GetComponent<LineRenderer>();
		lastShotTime = Time.time;
        spread = _weapon._baseSpread;
        speed = _baseSpeed;
    }

	void FixedUpdate()
	{
		mousePosition = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs (Camera.main.transform.position.y))); 	//Calculates the position of the mouse in WorldSpace (1)
		mouseDirection = Vector3.Normalize(mousePosition - rb.position);																						//Calculates the vector between the mouse and the player object		
        rb.velocity = Vector3.zero;
		Vector3 movementDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
                                                                                                                                                                    //sets the movement of the player            
        transform.Translate(movementDir * Time.fixedDeltaTime * speed, Space.World);
        rb.position = new Vector3
        (
            rb.position.x,
            0.0f,
            rb.position.z
        );
        transform.LookAt(mousePosition);          
    }

	void Update()
	{
        CalculateSpread();
        
        //DrawSpread(spread);

        CalculateAimIndicator();

        if (Input.GetButton("Fire1") && Time.time > (lastShotTime + _weapon._firerate) && ! _reloading)
        {
            if (!shotgunAus.isPlaying)
            {
                shotgunAus.clip = _weapon._shotSound;
            }
            ShootWeapon();
			lastShotTime = Time.time;
        }
        if (Input.GetButtonDown("Reload"))
        {
            if (_weapon.Reload())
            {
                _reloading = true;
                shotgunAus.clip = _weapon._reloadSound;
                shotgunAus.Play();
            }
        }
        if(shotgunAus.clip == _weapon._reloadSound && !shotgunAus.isPlaying)
        {
            _reloading = false;
        }
    }
    

    void ShootWeapon()
    {
        if (_weapon._ammoInClip > 0)
        {
            shotgunAus.Play();
            _weapon._ammoInClip--;
            for (int i = 0; i < _weapon._projectileCount; i++)
            {
                Fire(mousePosition, mouseDirection, spread, _weapon._dmg);
            }
            CalculateSpread(_weapon._shootSpread);
        }
    }

    


    void CalculateAimIndicator()
    {
        Vector3 pos;
        if (Vector3.Distance(transform.position, mousePosition) < 3.5f)
        {
            pos = transform.position + mouseDirection * 3.5f;
        }
        else
        {
            pos = mousePosition;
        }

        float width;
        width = Mathf.Tan(Mathf.Deg2Rad * (spread/2f)) * Vector3.Distance(rb.transform.position, pos) * 4f;
        lr.SetPosition(0, transform.position + (transform.forward * 1f));
        lr.SetPosition(1, pos);
        lr.SetWidth(Mathf.Tan(Mathf.Deg2Rad * (spread / 2f)) * 4f, width);
    }


    void CalculateSpread()
    {

        if (Input.GetButton("Fire2") && _weapon._baseSpread > _weapon._minSpread)
        {
            speed = _baseSpeed / 2;
            if (spread >= _weapon._minSpread)
                spread = spread - (_weapon._aimSpeed * Time.deltaTime);          
        }
        else if (!Input.GetButton("Fire1"))
        {
            spread = _weapon._baseSpread;
            speed = _baseSpeed;
        }
    }

    void CalculateSpread(float addSpread)
    {
        if (spread < _weapon._maxSpread)
        {
            spread += addSpread;
        }
        if (spread < _weapon._baseSpread)
        {
            spread = _weapon._baseSpread;
        }
    }


    void DrawSpread(float spread)
    {
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(spread, Vector3.up) * mouseDirection, Color.blue, Time.deltaTime);
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(-spread, Vector3.up) * mouseDirection, Color.blue, Time.deltaTime);
    }



    Vector3 CalculateSpreadVector(Vector3 lead,float spread)
    {
        return Quaternion.AngleAxis(Random.Range(-spread, spread), Vector3.up) * lead;
    }


	void Fire(Vector3 target, Vector3 targetDir, float spread, int dmg)																												//we nee a coroutine because you can't
	{                                                                                                                                           //delay in Update
        RaycastHit hit;
        Physics.Raycast(rb.position, CalculateSpreadVector(targetDir, spread), out hit, 100, canHit);                                                                                   //this Raycast determits if the player has hit an GameObject
        //Debug.DrawLine(rb.position, hit.point, Color.red, 1.5f);
        if (hit.collider != null) 
		{
            if (hit.collider.gameObject.CompareTag("enemy"))
            {                                                                   //is the GameObject an enemy?
                hit.collider.gameObject.GetComponent<Enemy>().Damage(dmg, hit.point);
            }
            else if (hit.collider.gameObject.CompareTag("obstacle"))
            {
                SpawnImpact(hit.point);
            }
            else
            {
                Debug.Log("Unexpected target was hit. Expect police investigations. We will take no responsibility.");
            }
		}


        MuzzleFlash();
	}


    private void SpawnImpact(Vector3 pos)
    {
        GameObject o = spawnPool.Spawn(pos, _impact);
        o.transform.rotation = rb.rotation;
    }


    private void MuzzleFlash()
    {
        _muzzle.GetComponent<Animator>().Play("MuzzleFlash");
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