using UnityEngine;
using System.Collections;

public interface Enemy
{	
	void Damage(int dmg, Vector3 at, Vector3 from);         //contains death animation/sound and objectpool managment

	void Talk();		//plays a random soundfile

}

public interface ObjectPoolable
{
    void SetObjectPool(ObjectPool o);

    ObjectPool GetObjectPool();
}
