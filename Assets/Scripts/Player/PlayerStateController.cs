using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : BaseController
{
    private Animator _playerStateMachine;
    private PlayerInput _playerInput;
    private PlayerMotorController _playerMotorController;
    private PlayerState _currentPlayerState;

    private bool _inBuildingMode = false;
    private bool _waitingForSecondPlayer = false;

    private float _currentWaitingForBuildingTimer = 0f;
    private bool _canEnterBuildingMode => _playerMotorController.OnGround && IsSecondPlayerNear() && PlayerController.COOPlayerReference.PlayerStateController.WaitingForSecondPlayer;
    private bool _canEnterWaitingForBuilding => _playerMotorController.OnGround && _currentWaitingForBuildingTimer <= 0f;

    public bool CanEnterBuildingMode => _canEnterBuildingMode;
    public bool WaitingForSecondPlayer => _waitingForSecondPlayer;

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



    public PlayerStateController(Animator playerStateMachine, PlayerController playerController
        , PlayerInput playerInput) : base(playerController)
    {
        _playerStateMachine = playerStateMachine;
        _playerInput = playerInput;
    }

    public override void DeInit()
    {

    }

    public override BaseController Init()
    {
        return this;
    }

    protected override void OnFixedUpdate()
    {

    }

    private bool IsSecondPlayerNear()
    {
        return Physics2D.Raycast(PlayerController.transform.position, Vector2.left, PlayerController.PlayerBuildingControllerData.BuildingEnableDistance, PlayerController.PlayerBuildingControllerData.COOPlayerLayerMask) || Physics2D.Raycast(PlayerController.transform.position, Vector2.right, PlayerController.PlayerBuildingControllerData.BuildingEnableDistance, PlayerController.PlayerBuildingControllerData.COOPlayerLayerMask);
    }

    protected override void OnUpdate()
    {
        if (!_inBuildingMode)
        {
            //this should be moved to something like PlayerStateController class
            if (_playerInput.Action)
            {
                if (_canEnterBuildingMode)
                {
                    Debug.LogWarning(PlayerController.gameObject.name + " Buduje");
                    PlayerController.COOPlayerReference.PlayerStateController.InBuildingMode = true;
                    _inBuildingMode = true;
                }
                else if (_canEnterWaitingForBuilding)
                {
                    Debug.LogWarning(PlayerController.gameObject.name + " Czekam");
                    _waitingForSecondPlayer = true;
                    _currentWaitingForBuildingTimer = PlayerController.PlayerBuildingControllerData.WaitingForBuildingTimer;
                }
            }

        }        

        if (_currentWaitingForBuildingTimer <= 0f && _waitingForSecondPlayer && !_inBuildingMode)
        {
            Debug.LogWarning(PlayerController.gameObject.name + " juz nie czekam");
            _waitingForSecondPlayer = false;
        }

        float v = _currentWaitingForBuildingTimer -= Time.deltaTime;
        _currentWaitingForBuildingTimer = Mathf.Clamp(v, 0, Mathf.Infinity);
    }

    public PlayerState GetCurrentPlayerState()
    {
        return _playerStateMachine.GetBehaviour<PlayerStateInfo>().PlayerState;
    }
}

public enum PlayerState
{
    Platforming,
    WaitingForAction,
    Building
}
