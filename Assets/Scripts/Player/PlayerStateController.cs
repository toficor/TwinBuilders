using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : BaseController
{
    private Animator _playerStateMachine;

    public PlayerStateController(Animator playerStateMachine, PlayerController playerController) : base(playerController)
    {
        _playerStateMachine = playerStateMachine;
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

    protected override void OnUpdate()
    {

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
