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

    private float _currentEnableBuildingTimer = 0f;

    // trzeba zrobic state WaitingForbuilding i referencje defaultowo do drugiego playera
    private bool _canEnterBuildingMode => IsSecondPlayerNear() && _playerMotorController.OnGround && _currentEnableBuildingTimer > 0f;

    public bool CanEnterBuildingMode => _canEnterBuildingMode;

    private bool _inBuildingMode = false;

    public bool InBuildingMode => _inBuildingMode;

    //TODO: mozna zrobic transform reference publiczne w player controller i wtedy kazdy konstruktor baseController w base bedzie potrzebowal do niego referencji
    public PlayerBuildingController(PlayerInput playerInput, PlayerEquipmentController playerEquipmentController, PlayerBuildingControllerData playerBuildingControllerData, PlayerMotorController playerMotorController, Transform playerTransform)
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

        Debug.LogError(_playerTransform.gameObject.name + "IsSecondPlayerNear: " + IsSecondPlayerNear());

        if (_playerInput.Build)
        {
            // jesli nie czeka to niech zacznie
            _currentEnableBuildingTimer = _playerBuildingControllerData.BuildingEnableTimer;
            //jesli drug gracz czeka to wchodza w build mode
        }

        if (_canEnterBuildingMode && !_inBuildingMode)
        {
            _currentEnableBuildingTimer = 0f;
            _inBuildingMode = true;
        }

        float v = _currentEnableBuildingTimer -= Time.deltaTime;
        _currentEnableBuildingTimer = Mathf.Clamp(v, 0, Mathf.Infinity);
    }

    private bool IsSecondPlayerNear()
    {
        return Physics2D.Raycast(_playerTransform.position, Vector2.left, _playerBuildingControllerData.BuildingEnableDistance, _playerBuildingControllerData.COOPlayerLayerMask) || Physics2D.Raycast(_playerTransform.position, Vector2.right, _playerBuildingControllerData.BuildingEnableDistance, _playerBuildingControllerData.COOPlayerLayerMask);
    }
}
