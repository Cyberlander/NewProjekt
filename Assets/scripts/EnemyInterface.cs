using UnityEngine;
using System.Collections;

public interface Enemy
{	
	void Die();         //contains death animation/sound and objectpool managment

	void Talk();		//plays a random soundfile

	void SetObjectPool(ObjectPool o);

	ObjectPool GetObjectPool();


}
