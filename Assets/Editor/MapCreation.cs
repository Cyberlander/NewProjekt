using UnityEngine;
using UnityEditor;
using System.Collections;

public class MapCreation : MonoBehaviour
{
    [MenuItem("MapCreation/Fence")]
    static void CreateFence()
    {
        Instantiate(Resources.Load("fencePart"));
    }

}
