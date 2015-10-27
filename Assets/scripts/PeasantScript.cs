using UnityEngine;
using System.Collections;

public class PeasantScript : MonoBehaviour 
{	
	[SerializeField]
	private GameObject target;
	[SerializeField]
	private float speed;
	private Rigidbody2D rb;
	private ParticleSystem ps;

	void Start () 
	{
		rb = GetComponent<Rigidbody2D> ();
		ps = GetComponent<ParticleSystem> ();
	}
	
	void Update () 
	{
		Vector3 dir = (target.transform.position - transform.position).normalized;
		Quaternion rotation = Quaternion.LookRotation
			(target.transform.position - transform.position, transform.TransformDirection(Vector3.up));
		transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);

		if (dir != transform.right) 
		{
			rb.MoveRotation(180);
		}
		rb.velocity = transform.right * speed;
		ps.emissionRate = 10 * speed;
		ps.startLifetime = 3 / speed;
	}
}
