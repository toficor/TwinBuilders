using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;
public class PlayerInput
{
    private Player _player;
    private PlayerControllerData _playerControllerData;

    public PlayerInput(PlayerControllerData playerControllerData)
    {
        _playerControllerData = playerControllerData;
        _player = ReInput.players.GetPlayer(_playerControllerData.RewiredPlayerId);
    }

    public float Horizontal
    {
        get
        {
            if(_player != null)
            {
                return _player.GetAxis("Horizontal");
            }

            return 0f;
        }
    }

    public bool Jump
    {
        get
        {
            if(_player != null)
            {
               return _player.GetButton("Jump");
            }

            return false;
        }
    }

    public bool Build
    {
        get 
        {
            if(_player != null)
            {
                return _player.GetButton("Build");
            }
            return false;
        }
    }

}
