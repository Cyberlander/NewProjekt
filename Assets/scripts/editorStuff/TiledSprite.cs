using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class TiledSprite : MonoBehaviour
{
    public Vector2 _dimensions;
    public Sprite _spriteTexture;

    [HideInInspector][SerializeField]
    private Vector2 _currentDimensions;

    [HideInInspector][SerializeField]
    private List<GameObject> _sprites;
    

    void Update()
    {
        if (!_currentDimensions.Equals(_dimensions))
        {
            if(_currentDimensions.x < _dimensions.x)
            {
                
            }
        }
    }
	
}
