using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingController : BaseController
{
    private PlayerEquipmentController _playerEquipmentController;
    private PlayerInput _playerInput;

    public PlayerBuildingController(PlayerInput playerInput, PlayerEquipmentController playerEquipmentController)
    {
        _playerInput = playerInput;
        _playerEquipmentController = playerEquipmentController;
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void DeActivate()
    {
        base.DeActivate();
    }

    public override void DeInit()
    {
       // throw new System.NotImplementedException();
    }

    public override BaseController Init()
    {
        return this;
    }

    protected override void OnFixedUpdate()
    {
        
    }

    protected override void OnUpdate()
    {
       // core loop of controller;
    }
}
