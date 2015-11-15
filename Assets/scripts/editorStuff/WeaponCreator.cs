using UnityEngine;
using System.Collections;
using UnityEditor;

public class WeaponCreator
{
    [MenuItem("Assets/Create/Weapon Object")]
    public static void Create()
    {
        WeaponObject asset = ScriptableObject.CreateInstance<WeaponObject>();
        AssetDatabase.CreateAsset(asset, "Assets/NewWeaponObject.asset");
    }
	
}
