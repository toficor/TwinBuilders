using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingController : BaseController
{
    private PlayerEquipmentController _playerEquipmentController;
    private PlayerInput _playerInput;
    private Transform _playerTransform;
    private PlayerBuildingControllerData _playerBuildingControllerData;
    private PlayerMotorController _playerMotorController;

    private float _currentWaitingForBuildingTimer = 0f;
    private PlayerController _cOOPlayerReference;

    private bool _inBuildingMode = false;
    private bool _waitingForSecondPlayer = false;
    private bool _canEnterBuildingMode => _playerMotorController.OnGround && IsSecondPlayerNear() && _cOOPlayerReference.PlayerBuildingController.WaitingForSecondPlayer;

    private bool _canEnterWaitingForBuilding => _playerMotorController.OnGround && _currentWaitingForBuildingTimer <= 0f;

    public bool CanEnterBuildingMode => _canEnterBuildingMode;
    public bool WaitingForSecondPlayer => _waitingForSecondPlayer;



    public bool InBuildingMode => _inBuildingMode;

    //TODO: mozna zrobic transform reference publiczne w player controller i wtedy kazdy konstruktor baseController w base bedzie potrzebowal do niego referencji
    public PlayerBuildingController(PlayerInput playerInput,
                                    PlayerEquipmentController playerEquipmentController,
                                    PlayerBuildingControllerData playerBuildingControllerData,
                                    PlayerMotorController playerMotorController,
                                    Transform playerTransform,
                                    PlayerController playerController): base (playerController)
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

        if (_playerInput.Build)
        {
            if (_canEnterBuildingMode)
            {
               // Debug.LogError("Buduje");
                _cOOPlayerReference.PlayerBuildingController._inBuildingMode = true;
                _inBuildingMode = true;
            }
            else if (_canEnterWaitingForBuilding)
            {
               // Debug.LogError("Czekam");
                _waitingForSecondPlayer = true;
                _currentWaitingForBuildingTimer = _playerBuildingControllerData.WaitingForBuildingTimer;
            }
        }

        if (_currentWaitingForBuildingTimer <= 0f && _waitingForSecondPlayer && !_inBuildingMode)
        {
          //  Debug.LogError("juz nie czekam");
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

}
