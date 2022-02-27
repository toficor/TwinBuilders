using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerEquipmentData", menuName = "Data/PlayerEquipmentData", order = 1)]
public class PlayerEquipmentData : ScriptableObject
{
    // public List<PlaceableObjectData> PlaceableObjectDatas = new List<PlaceableObjectData>();
    public PlayerShapeData BasePlayerShape;
    public List<PlayerShapeData> TransformingPlayerShapes = new List<PlayerShapeData>();

}

[Serializable]
public class PlayerShapeData
{
    public float Width = 1f;
    public float Height = 1f;
    public Sprite Sprite;
    public LayerMask LayerMask;
}


