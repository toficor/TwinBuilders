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

    private float _currentWaitingForBuildingTimer = 0f;
    private float _changingShapeTimer = 0f;
    private PlayerController _cOOPlayerReference;

    private bool _inBuildingMode = false;
    private bool _waitingForSecondPlayer = false;
    private bool _canEnterBuildingMode => _playerMotorController.OnGround && IsSecondPlayerNear() && _cOOPlayerReference.PlayerBuildingController.WaitingForSecondPlayer;

    private bool _canEnterWaitingForBuilding => _playerMotorController.OnGround && _currentWaitingForBuildingTimer <= 0f;

    public bool CanEnterBuildingMode => _canEnterBuildingMode;
    public bool WaitingForSecondPlayer => _waitingForSecondPlayer;

    public Action<PlayerShapeData> OnPlayerChangeShape = delegate (PlayerShapeData newPlayerShape) { };

    public bool InBuildingMode
    {
        get
        {
            return _inBuildingMode;
        }

        set
        {
            _inBuildingMode = value;
        }
    }

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
        _cOOPlayerReference = PlayerController.ImFirstPlayer ? GameManager.Instance.SecondPlayerReference : GameManager.Instance.FirstPlayerReference;
        Debug.LogWarning("Me: " + PlayerController.gameObject.name + " my Coop: " + _cOOPlayerReference.gameObject.name);
        return this;
    }

    protected override void OnFixedUpdate()
    {

    }

    protected override void OnUpdate()
    {

        // Debug.LogError(_playerTransform.gameObject.name + "IsSecondPlayerNear: " + IsSecondPlayerNear());

        if (!_inBuildingMode)
        {
            //this should be moved to something like PlayerStateController class
            if (_playerInput.Build)
            {
                if (_canEnterBuildingMode)
                {
                    Debug.LogWarning(PlayerController.gameObject.name + " Buduje");
                    _cOOPlayerReference.PlayerBuildingController._inBuildingMode = true;
                    _inBuildingMode = true;
                }
                else if (_canEnterWaitingForBuilding)
                {
                    Debug.LogWarning(PlayerController.gameObject.name + " Czekam");
                    _waitingForSecondPlayer = true;
                    _currentWaitingForBuildingTimer = _playerBuildingControllerData.WaitingForBuildingTimer;
                }
            }

        }
        else
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

        if (_currentWaitingForBuildingTimer <= 0f && _waitingForSecondPlayer && !_inBuildingMode)
        {
            Debug.LogWarning(PlayerController.gameObject.name + " juz nie czekam");
            _waitingForSecondPlayer = false;
        }
        //if (_canEnterBuildingMode && !_inBuildingMode)
        //{
        //    _currentEnableBuildingTimer = 0f;
        //    _inBuildingMode = true;
        //}



        float v = _currentWaitingForBuildingTimer -= Time.deltaTime;
        _currentWaitingForBuildingTimer = Mathf.Clamp(v, 0, Mathf.Infinity);
    }

    private bool IsSecondPlayerNear()
    {
        return Physics2D.Raycast(_playerTransform.position, Vector2.left, _playerBuildingControllerData.BuildingEnableDistance, _playerBuildingControllerData.COOPlayerLayerMask) || Physics2D.Raycast(_playerTransform.position, Vector2.right, _playerBuildingControllerData.BuildingEnableDistance, _playerBuildingControllerData.COOPlayerLayerMask);
    }

    private void SteeringPlayerBehaviour()
    {
        //35 for debug 
        _cOOPlayerReference.PlayerMotorController.MoveCharacter(new Vector2(_playerInput.Horizontal * 35f, _playerInput.Vertical * 35f));


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
            _cOOPlayerReference.PlayerBuildingController.InBuildingMode = false;
            _abbleToReturn = true;
        }

        if (_playerInput.Build && _abbleToReturn)
        {
            OnPlayerChangeShape(_playerEquipmentController.GetPlayerDefaultShapeData());
            PlayerController.transform.position = _cOOPlayerReference.GetNearestAvailablePosition();
            _playerMotorController.UnFreezConstrains();
            _inBuildingMode = false;
            PlayerController.PlayerRigidbody.gravityScale = 8f;
            PlayerController.PlayerRigidbody.drag = 10f;
        }


    }

}
