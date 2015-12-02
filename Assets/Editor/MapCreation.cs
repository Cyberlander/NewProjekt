using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapCreation : MonoBehaviour
{
    [MenuItem("MapCreation/Fence")]
    [MenuItem("GameObject/MapCreation/Fence")]
    static void CreateFence()
    {
        Instantiate(Resources.Load("fence"));
    }

    [MenuItem("MapCreation/Map")]
    [MenuItem("GameObject/MapCreation/Map")]
    static void CreateMap()
    {
        MapObject asset = ScriptableObject.CreateInstance<MapObject>();
        AssetDatabase.CreateAsset(asset, "Assets/_scenes/maps/NewMap.asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
