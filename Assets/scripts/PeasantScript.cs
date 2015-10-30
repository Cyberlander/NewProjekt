using UnityEngine;
using System.Collections;

public class PeasantScript : MonoBehaviour, Enemy
{	
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private float speed;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private GameObject deathAnim;

	private Rigidbody2D rb;
	private ParticleSystem ps;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		ps = GetComponent<ParticleSystem> ();
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
	}

	

	public void Die()
	{
		sprite.enabled = false;
		rb.AddTorque (Random.Range (-1, 1));
		deathAnim.GetComponent<Animator> ().Play ("peasantDeathAnim");

		StartCoroutine(DisableIn (0.117f));
				
	}

	IEnumerator DisableIn(float sec)
	{
		yield return new WaitForSeconds(sec);
		gameObject.SetActive(false);
	}


}
