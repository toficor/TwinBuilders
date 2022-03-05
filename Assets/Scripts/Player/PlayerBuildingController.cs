using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerBuildingController : BaseController
{
    private PlayerEquipmentController _playerEquipmentController;
    private PlayerInput _playerInput;
    private Transform _playerTransform;
    private PlayerBuildingControllerData _playerBuildingControllerData;
    private PlayerMotorController _playerMotorController;
    private PlayerStateController _playerStateController;

    private float _changingShapeTimer = 0f;

    public Action<PlayerShapeData> OnPlayerChangeShape = delegate (PlayerShapeData newPlayerShape) { };

    public PlayerBuildingController(PlayerInput playerInput,
                                    PlayerEquipmentController playerEquipmentController,
                                    PlayerBuildingControllerData playerBuildingControllerData,
                                    PlayerMotorController playerMotorController,
                                    Transform playerTransform,
                                    PlayerController playerController) : base(playerController)
    {
        _playerInput = playerInput;
        _playerEquipmentController = playerEquipmentController;
        _playerTransform = playerTransform;
        _playerBuildingControllerData = playerBuildingControllerData;
        _playerMotorController = playerMotorController;
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
        if (PlayerController.ImFirstPlayer)
        {
            SteeringPlayerBehaviour();
        }
        else
        {
            BuildingPlayerBehaviour();
        }
    }



    private void SteeringPlayerBehaviour()
    {
        //35 for debug 
        PlayerController.COOPlayerReference.PlayerMotorController.MoveCharacter(new Vector2(_playerInput.Horizontal * 35f, _playerInput.Vertical * 35f));
    }

    int _index = 0;
    bool _abbleToReturn = false;
    private void BuildingPlayerBehaviour()
    {
        PlayerController.PlayerRigidbody.gravityScale = 0f;
        PlayerController.PlayerRigidbody.drag = 0f;

        _changingShapeTimer += Time.deltaTime;

        if (_playerInput.Horizontal > 0 || _playerInput.Horizontal < 0)
        {
            if (_changingShapeTimer >= _playerBuildingControllerData.TimeBetweenChangingShapes)
            {
                _index = Mathf.Clamp(_index += Math.Sign(_playerInput.Horizontal), 0, _playerEquipmentController.GetPlayerShapesCount() - 1);
                OnPlayerChangeShape(_playerEquipmentController.GetPlayerShapeData(_index));
                _changingShapeTimer = 0f;
            }
        }

        //jump unless player state controller done
        if (_playerInput.Jump)
        {
            _playerMotorController.FreezConstrains();
            PlayerController.COOPlayerReference.PlayerStateController.InBuildingMode = false;
            _abbleToReturn = true;
        }

        if (_playerInput.Action && _abbleToReturn)
        {
            OnPlayerChangeShape(_playerEquipmentController.GetPlayerDefaultShapeData());
            PlayerController.transform.position = PlayerController.COOPlayerReference.GetNearestAvailablePosition();
            _playerMotorController.UnFreezConstrains();
            PlayerController.PlayerStateController.InBuildingMode = false;
            PlayerController.PlayerRigidbody.gravityScale = 8f;
            PlayerController.PlayerRigidbody.drag = 10f;
        }


    }

}
