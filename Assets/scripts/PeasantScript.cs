using UnityEngine;
using System.Collections;

public class PeasantScript : MonoBehaviour, Enemy, ObjectPoolable
{	
	[SerializeField]
	private float speed;
    [SerializeField]
    private float _viewRange;
    [SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private GameObject deathAnim;
	[SerializeField]
	private AudioClip[] clips;
    [SerializeField]
    private GameObject _bloodsplatter;
    [SerializeField]
    private GameObject _bloodstain;

	private ParticleSystem ps;
	private AudioSource aus;
	private GameObject target;
    private Rigidbody _targetRB;
	private ObjectPool _op;
	private int health;
    private NavMeshAgent _nma;
 

	void Awake () 
	{
		ps = GetComponent<ParticleSystem> ();
		aus = GetComponent<AudioSource> ();
        _nma = GetComponent<NavMeshAgent>();
		health = 50;
		target = GameObject.FindGameObjectWithTag ("Player");
        _targetRB = target.GetComponent<Rigidbody>();
	}

  
	
	void Update () 
	{
        CalculateBehaviour();
        //ShowPath(_nma.path);                                                                                                   // lets the GameObjekt move forwards	
        ps.emissionRate = 10 * speed;																		// dynamic adaption of the Particlesystem's parameters to make the length and look of -->
		ps.startLifetime = 3 / speed;																		// trail independent from the GameObjects speed
		

        if (Vector3.Distance(_targetRB.position, transform.position) < 0.6)
        {
            Application.LoadLevel("fail");
        }
    }

    private void CalculateBehaviour()
    {       
        Follow(target);
              
        if (Vector3.Distance(transform.position, _targetRB.position) < _viewRange)
        {
            _nma.speed = speed * 2;
            if (!aus.isPlaying)
                Talk();
        }
        else
        {
            _nma.speed = speed;
        }
        
    } 
   
   

    private void Follow(GameObject t)
    {
        
        _nma.SetDestination(t.transform.position);
    }

    private void ShowPath(NavMeshPath path)
    {
        Vector3[] corners = path.corners;

        for(int i = 1; i < corners.Length; i++)
        {
            Debug.DrawLine(corners[i - 1], corners[i], Color.green, Time.fixedDeltaTime);
        }
        
    }

	public void Damage(int dmg, Vector3 at, Vector3 from)
	{
		health = health - dmg;
        GameObject bs = _op.Spawn(at + from * 0.1f, _bloodsplatter);
        bs.transform.rotation = FaceObject(-from, FacingDirection.RIGHT);
        if (health <= 0)
			Die ();
	}

	private void Die()
	{
		sprite.enabled = false;																				//-
		ps.Stop ();																							//-
		ps.Clear ();																						//makes the peasant invisible
		health = 50;																
		deathAnim.GetComponent<Animator> ().Play ("peasantDeathAnim");	                                    //plays the death animation
        _op.Spawn(transform.position,_bloodstain);
		StartCoroutine(DisableIn(0.2f));
				
	}
	IEnumerator DisableIn(float sec)																		// disables the GameObject after som delay. this was the animation won't be interupted
	{
		yield return new WaitForSeconds(sec);
		sprite.enabled = true;
        deathAnim.GetComponent<SpriteRenderer>().sprite = null;
		_op.Despawn(gameObject);
	}

	public void SetObjectPool(ObjectPool o)
	{
		_op = o;
	}
	public ObjectPool GetObjectPool()
	{
		return _op;
	}


	public void Talk()
	{
			aus.clip = clips [Random.Range (0, clips.Length)];
			aus.Play ();
	}


    private enum FacingDirection
    {
        UP = 270,
        DOWN = 90,
        LEFT = 180,
        RIGHT = 0
    }

    private static Quaternion FaceObject(Vector3 direction, FacingDirection facing)
    {
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        angle -= (float)facing;
        return Quaternion.AngleAxis(angle, Vector3.up);
    }
}
