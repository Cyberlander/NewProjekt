using UnityEngine;
using System.Collections;

public class PeasantScript : MonoBehaviour, Enemy
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
	private ParticleSystem ps;
	private AudioSource aus;
	private GameObject target;
	private ObjectPool op;
	private int health;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		ps = GetComponent<ParticleSystem> ();
		aus = GetComponent<AudioSource> ();
		health = 50;
		target = GameObject.FindGameObjectWithTag ("Player");
	}
	
	void Update () 
	{
		Vector3 dir = (target.transform.position - transform.position).normalized;							// calculates the Vector pointing from this GameObject towards the target
		Quaternion rotation = Quaternion.LookRotation
			(target.transform.position - transform.position, transform.TransformDirection(Vector3.up));		// creates a quaternion which describes the rotation needed make the GameObjekt face the direction -->
																											// the Vector dir is pointing
		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);									// applies the rotation

		if (dir != transform.right) 																		// fixes the bug which makes the GamObject flip directions, pointing away from the target
		{
			rb.MoveRotation(180);
		}
		rb.velocity = transform.right * speed;																// lets the GameObjekt move forwards	
		ps.emissionRate = 10 * speed;																		// dynamic adaption of the Particlesystem's parameters to make the length and look of -->
		ps.startLifetime = 3 / speed;																		// trail independent from the GameObjects speed


		if (Vector3.Distance(target.transform.position, transform.position) < 3 && !aus.isPlaying)
			Talk ();
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
		ps.Stop ();																							//-
		ps.Clear ();																						//makes the peasant invisible
		health = 50;
		rb.AddTorque (Random.Range (-1, 1));																//randomizes the death animation																
		deathAnim.GetComponent<Animator> ().Play ("peasantDeathAnim");										//plays the death animation
		StartCoroutine(DisableIn(0.2f));
				
	}
	IEnumerator DisableIn(float sec)																		// disables the GameObject after som delay. this was the animation won't be interupted
	{
		yield return new WaitForSeconds(sec);
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



}
