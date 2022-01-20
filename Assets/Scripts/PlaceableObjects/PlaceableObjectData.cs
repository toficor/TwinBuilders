using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlaceableObjectData", menuName = "Data/PlaceableObjectData", order = 1)]
public class PlaceableObjectData : ScriptableObject
{
    public int Height;
    public int Width;
    public GameObject Prefab;
}
