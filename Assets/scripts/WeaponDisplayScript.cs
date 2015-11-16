using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WeaponDisplayScript : MonoBehaviour
{
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _weaponTypeText;

    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        _ammoText.text = _player.GetComponent<PlayerControllerScript>()._weapon._ammoInClip + " / " + _player.GetComponent<PlayerControllerScript>()._weapon._ammo;
        _weaponTypeText.text = _player.GetComponent<PlayerControllerScript>()._weapon._name;
    }
}
