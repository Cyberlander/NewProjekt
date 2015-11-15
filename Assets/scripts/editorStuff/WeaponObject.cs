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
    public float _clipSize = 10;
    public float _ammoInClip = 10;
    public float _ammo = 50;
    public float _firerate = 2f;
    public float _reloadTime = 3f;
    public Sprite _graphic;
    public AudioClip _shotSound;

    public bool Reload()
    {
        if (_ammo == 0)
        {
            return false;
        }
        else
        {
            float remainingAmmo = Mathf.Clamp(_ammo - (_clipSize - _ammoInClip), 0, _ammo);
            _ammoInClip = Mathf.Clamp(_ammo, 0, _clipSize);
            _ammo = remainingAmmo;
            return true;
        }
    }
}
