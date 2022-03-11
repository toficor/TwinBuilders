using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateInfo : StateMachineBehaviour
{
    [SerializeField] private PlayerState _playerState;
    private PlayerController _playerController;
    public PlayerState PlayerState => _playerState;

    private bool _firstEntry = true;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_firstEntry)
        {
            _firstEntry = false;
            _playerController = animator.GetComponent<PlayerController>();
        }

        _playerController?.PlayerStateController.OnPlayerEnterAnyState.Invoke(_playerState);

        switch (_playerState)
        {
            case PlayerState.Platforming:
                _playerController?.PlayerStateController.OnPlayerEnterPlatformingMode.Invoke();
                break;
            case PlayerState.WaitingForAction:
                _playerController?.PlayerStateController.OnPlayerEnterWaitingMode.Invoke();
                break;
            case PlayerState.Building:
                _playerController?.PlayerStateController.OnPlayerEnterBuildingMode.Invoke();
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerController?.PlayerStateController.OnPlayerExitAnyState.Invoke(_playerState);

        switch (_playerState)
        {
            case PlayerState.Platforming:
                _playerController?.PlayerStateController.OnPlayerExitPlatformingMode.Invoke();
                break;
            case PlayerState.WaitingForAction:
                _playerController?.PlayerStateController.OnPlayerExitWaitingMode.Invoke();
                break;
            case PlayerState.Building:
                _playerController?.PlayerStateController.OnPlayerExitBuildingMode.Invoke();
                break;
            case PlayerState.AfterBuilding:
                _playerController?.PlayerStateController.OnPlayerExitAfterBuilding.Invoke();
                break;
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
