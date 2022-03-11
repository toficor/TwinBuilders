using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStateController : BaseController
{
    public Action OnPlayerEnterBuildingMode = delegate { };
    public Action OnPlayerExitBuildingMode = delegate { };

    public Action OnPlayerEnterWaitingMode = delegate { };
    public Action OnPlayerExitWaitingMode = delegate { };

    public Action OnPlayerEnterPlatformingMode = delegate { };
    public Action OnPlayerExitPlatformingMode = delegate { };

    public Action OnPlayerEnterAfterBuildingMode = delegate { };
    public Action OnPlayerExitAfterBuilding = delegate { };

    public Action<PlayerState> OnPlayerEnterAnyState;

    private Animator _playerStateMachine;
    private PlayerInput _playerInput;
    private PlayerState _currentPlayerState;

    private float _currentWaitingForBuildingTimer = 0f;
    private bool _canEnterBuildingMode
    {
        get
        {

            //  Debug.LogError(PlayerController.COOPlayerReference.name + " waiting: " + PlayerController.COOPlayerReference.PlayerStateController.WaitingForSecondPlayer);

            return PlayerController.PlayerMotorController.OnGround && IsSecondPlayerNear() && PlayerController.COOPlayerReference.PlayerStateController.WaitingForSecondPlayer;
        }
    }
    private bool _canEnterWaitingForBuilding => PlayerController.PlayerMotorController.OnGround && _currentWaitingForBuildingTimer <= 0f;

    public bool CanEnterBuildingMode => _canEnterBuildingMode;
    public bool WaitingForSecondPlayer => GetCurrentPlayerState() == PlayerState.WaitingForAction;

    public bool InBuildingMode => GetCurrentPlayerState() == PlayerState.Building;

    public bool InAfterBuilingState => GetCurrentPlayerState() == PlayerState.AfterBuilding;

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
        OnPlayerEnterAnyState += SetCurrentPLayerState;
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
        switch (_currentPlayerState)
        {
            case PlayerState.Platforming:
                if (_playerInput.Action && _canEnterWaitingForBuilding)
                {
                    _currentWaitingForBuildingTimer = PlayerController.PlayerBuildingControllerData.WaitingForBuildingTimer;
                    _playerStateMachine.SetTrigger("IsWaitingForAction");
                }
                break;
            case PlayerState.WaitingForAction:
                if (_playerInput.Action && _canEnterBuildingMode)
                {
                    _playerStateMachine.SetBool("IsBuilding", true);
                }
                if (_playerInput.Cancel)
                {
                    _currentWaitingForBuildingTimer = 0;
                    _playerStateMachine.SetTrigger("EndWaitingForAction");
                }
                if (_currentWaitingForBuildingTimer <= 0f)
                {
                    _playerStateMachine.SetTrigger("EndWaitingForAction");
                }

                float v = _currentWaitingForBuildingTimer -= Time.deltaTime;
                _currentWaitingForBuildingTimer = Mathf.Clamp(v, 0, Mathf.Infinity);

                break;
            case PlayerState.Building:
                if (_playerInput.Cancel)
                {
                    _playerStateMachine.SetBool("IsBuilding", false);
                }
                if (_playerInput.Action)
                {
                    _playerStateMachine.SetTrigger("AfterBuilding");
                }
                break;
            case PlayerState.AfterBuilding:
                if (_playerInput.Cancel)
                {
                    _playerStateMachine.SetBool("IsBuilding", false);
                }
                break;
        }  

    }

    // ja wiem jak to wyglada, ale unity animator jest zjebany i trzeba to zrobic tak
    public void SetCurrentPLayerState(PlayerState playerState)
    {
        Debug.LogWarning("ustawiam stan na " + playerState);
        _currentPlayerState = playerState;
    }

    public PlayerState GetCurrentPlayerState()
    {
        return _currentPlayerState;
    }
}

public enum PlayerState
{
    Platforming,
    WaitingForAction,
    Building,
    AfterBuilding,
}
