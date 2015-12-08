using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class fatPeasantScript : MonoBehaviour, Enemy, ObjectPoolable
{	
	[SerializeField]
	private float speed;
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

	private AudioSource aus;
	private GameObject target;
	private ObjectPool _op;
	private int health;
    private NavMeshAgent _nma;

    void Start () 
	{
		aus = GetComponent<AudioSource> ();
		target = GameObject.FindGameObjectWithTag ("Player");
        _nma = GetComponent<NavMeshAgent>();
		health = 250;
    }
	
	void Update () 
	{
        Move();
        //ShowPath(_nma.path);

        if (Vector3.Distance(target.transform.position, transform.position) < 4 && !aus.isPlaying)
			Talk ();

        if (Vector3.Distance(target.transform.position, transform.position) < 1)
        {
            SceneManager.LoadScene("fail");
        }
	}

    private void Move()
    {

        _nma.SetDestination(target.transform.position);
        _nma.speed = speed;
    }

    private void ShowPath(NavMeshPath path)
    {
        Vector3[] corners = path.corners;

        for (int i = 1; i < corners.Length; i++)
        {
            Debug.DrawLine(corners[i - 1], corners[i], Color.green, Time.fixedDeltaTime);
        }

    }


    public void Damage(int dmg, Vector3 at, Vector3 from)
	{
		health = health - dmg;
        GameObject bs = _op.Spawn(at + from * 1.4f, _bloodsplatter);
        bs.transform.rotation = FaceObject(-from, FacingDirection.RIGHT);
        if (health <= 0)
			Die ();
	}
	
	private void Die()
	{
		sprite.enabled = false;																				
		health = 250;																																
		deathAnim.GetComponent<Animator> ().Play ("peasantDeathAnim");										//plays the death animation
        _op.Spawn(transform.position, _bloodstain);
        StartCoroutine(DisableIn(0.2f));
		
	}
	IEnumerator DisableIn(float sec)																		// disables the GameObject after som delay. this was the animation won't be interupted
	{
		yield return new WaitForSeconds(sec);
        deathAnim.GetComponent<SpriteRenderer>().sprite = null;
        sprite.enabled = true;
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