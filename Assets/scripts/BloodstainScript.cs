using UnityEngine;
using System.Collections;

public class BloodstainScript : MonoBehaviour, ObjectPoolable
{
    [SerializeField]
    private float _showForSeconds;
    [SerializeField]
    private Vector3 _maxSize;

    private ObjectPool _op;
    private SpriteRenderer _sr;
    private float _lifetimeLeft;   

   

    public void SetObjectPool(ObjectPool o)
    {
        _op = o;
    }

    public ObjectPool GetObjectPool()
    {
        return _op;
    }

    void Awake()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {       
        _sr.color = new Color(1,1,1,0.9f);
        gameObject.transform.Rotate(0f, Random.Range(0f, 359f), 0f);
        _lifetimeLeft = _showForSeconds;
        transform.localScale = _maxSize * 0.5f;
    }

    void Update()
    {
        if(_lifetimeLeft < _showForSeconds/3)
        {
            _sr.color = new Color(1, 1, 1, _sr.color.a - 1/ (_showForSeconds / 3) * Time.deltaTime);
        }
        if (transform.localScale.x < _maxSize.x)
        {
            transform.localScale = transform.localScale + transform.localScale * Time.deltaTime;
        }
        if(_lifetimeLeft <= 0 || _sr.color.a <= 0)
        {
            _op.Despawn(gameObject);
        }       
        _lifetimeLeft -= Time.deltaTime;
    }
}
