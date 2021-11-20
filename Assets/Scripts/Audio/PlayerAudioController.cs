using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class PlayerAudioController : MonoBehaviour
{

    [EventRef]
    public string FootstepsEvent = "";
    [EventRef]
    public string JumpingEvent = "";
    [EventRef]
    public string JumpingExhaleEvent = "";

    private PlayerMotor _playerMotor;

    public void Init(PlayerMotor playerMotor)
    {
        _playerMotor = playerMotor;

        playerMotor.OnJump += PlayJumpEvent;
        playerMotor.OnJump += PlayJumpExhaleEvent;
    }

    public void PlayFootstepsEvent()
    {
        RuntimeManager.PlayOneShot(FootstepsEvent);
    }

    public void PlayJumpEvent()
    {
        RuntimeManager.PlayOneShot(JumpingEvent);
    }

    public void PlayJumpExhaleEvent()
    {
        RuntimeManager.PlayOneShot(JumpingExhaleEvent);
    }
}
