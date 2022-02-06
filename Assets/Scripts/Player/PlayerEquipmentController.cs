using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentController : BaseController
{
    private PlayerEquipmentData _playerEquipmentData;
    private PlayerInput _playerInput;

    public PlayerEquipmentController(PlayerInput playerInput, PlayerEquipmentData playerEquipmentData)
    {
        _playerInput = playerInput;
        _playerEquipmentData = playerEquipmentData;
    }

    public PlaceableObjectData GetPlaceableItem(int index)
    {
        return _playerEquipmentData.PlaceableObjectDatas[index];
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
