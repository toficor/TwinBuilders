using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController : BaseController
{
    private PlayerEquipmentData _playerEquipmentData;
    private PlayerInput _playerInput;

    public PlayerEquipmentController(PlayerInput playerInput,
                                     PlayerEquipmentData playerEquipmentData,
                                     PlayerController playerController) : base(playerController)
    {
        _playerInput = playerInput;
        _playerEquipmentData = playerEquipmentData;
    }

    //public PlaceableObjectData GetPlaceableItem(int index)
    //{
    //    return _playerEquipmentData.PlaceableObjectDatas[index];
    //}

    public Sprite GetShape(int index)
    {
        return GetPlayerShapeData(index).Sprite;
    }

    public Sprite GetPlayerDefaultShapeSprite()
    {
        return GetPlayerDefaultShapeData().Sprite;
    }

    public PlayerShapeData GetPlayerShapeData(int index)
    {
        return _playerEquipmentData.TransformingPlayerShapes[index];
    }
    public PlayerShapeData GetPlayerDefaultShapeData()
    {
        return _playerEquipmentData.BasePlayerShape;
    }


    public int GetPlayerShapesCount()
    {
        return _playerEquipmentData.TransformingPlayerShapes.Capacity;
    }

    public override BaseController Init()
    {
        return this;
    }

    public override void DeInit()
    {

    }

    protected override void OnUpdate()
    {
        //core loop of controller 
    }

    protected override void OnFixedUpdate()
    {

    }
}
