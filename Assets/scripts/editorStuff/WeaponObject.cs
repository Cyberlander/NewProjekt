using UnityEngine;
using System.Collections;


[System.Serializable]
public class WeaponObject : ScriptableObject
{
    public string _name = "Weapon Name";
    public int _dmg = 0;
    public float _spread = 10;
    public float _shootSpread = 0;
    public int _projectileCount = 1;
    public int _clipSize = 10;
    public float _firerate = 2f;
    public Sprite _graphic;
    public AudioClip _shotSound;

}
