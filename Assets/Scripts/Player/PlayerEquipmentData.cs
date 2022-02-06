using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerEquipmentData", menuName = "Data/PlayerEquipmentData", order = 1)]
public class PlayerEquipmentData : ScriptableObject
{
    public List<PlaceableObjectData> PlaceableObjectDatas = new List<PlaceableObjectData>();
}


