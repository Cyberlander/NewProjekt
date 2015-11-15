using UnityEngine;
using System.Collections;

public class ImpactVfxScript : MonoBehaviour, ObjectPoolable
{
    ObjectPool _op;
    ParticleSystem _ps;

    void Start()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {

        if (_ps != null)
        {
            if (_ps.isStopped)
            {
                _op.Despawn(gameObject);
            }
        }
    }

    public void SetObjectPool(ObjectPool o)
    {
        _op = o;
    }

    public ObjectPool GetObjectPool()
    {
        return _op;
    }
}
