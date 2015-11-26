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

}
