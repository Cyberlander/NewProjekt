using UnityEngine;
using System.Collections;

public class fatPeasantScript : MonoBehaviour, Enemy
{	
	[SerializeField]
	private float speed;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private GameObject deathAnim;
	[SerializeField]
	private AudioClip[] clips;
	
	private Rigidbody2D rb;
	private AudioSource aus;
	private GameObject target;
	private ObjectPool op;
	private int health;
	
	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		aus = GetComponent<AudioSource> ();
		target = GameObject.FindGameObjectWithTag ("Player");
		health = 100;
	}
	
	void Update () 
	{
        transform.rotation = FaceObject(transform.position, target.transform.position, FacingDirection.RIGHT);
		rb.velocity = transform.right * speed;																// lets the GameObjekt move forwards	
			
		if (Vector3.Distance(target.transform.position, transform.position) < 4 && !aus.isPlaying)
			Talk ();

        if (Vector3.Distance(target.transform.position, transform.position) < 1)
        {
            Application.LoadLevel("fail");
        }
	}
	

	public void Damage(int dmg)
	{
		health = health - dmg;
		if (health <= 0)
			Die ();
	}
	
	private void Die()
	{
		sprite.enabled = false;																				//-
		health = 100;
		rb.AddTorque (Random.Range (-1, 1));																//randomizes the death animation																
		deathAnim.GetComponent<Animator> ().Play ("peasantDeathAnim");										//plays the death animation
		StartCoroutine(DisableIn(0.2f));
		
	}
	IEnumerator DisableIn(float sec)																		// disables the GameObject after som delay. this was the animation won't be interupted
	{
		yield return new WaitForSeconds(sec);
        deathAnim.GetComponent<SpriteRenderer>().sprite = null;
        sprite.enabled = true;
		op.Despawn(gameObject);
	}
	
	public void SetObjectPool(ObjectPool o)
	{
		op = o;
	}
	public ObjectPool GetObjectPool()
	{
		return op;
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

    private static Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition, FacingDirection facing)
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= (float)facing;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
}