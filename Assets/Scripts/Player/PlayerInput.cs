using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;
public class PlayerInput
{
    private Player _player;
    private PlayerControllerData _playerControllerData;

    public Action<bool> OnPlayerToggleBuildingMode = delegate (bool buildingModeOn) { };

    public PlayerInput(PlayerControllerData playerControllerData)
    {
        _playerControllerData = playerControllerData;
        _player = ReInput.players.GetPlayer(_playerControllerData.RewiredPlayerId);
    }

    public float Horizontal
    {
        get
        {
            if (_player != null)
            {
                return _player.GetAxis("Horizontal");
            }

            return 0f;
        }
    }

    public float Vertical
    {
        get
        {
            if (_player != null)
            {
                return _player.GetAxis("Vertical");
            }

            return 0f;
        }
    }

    public bool Jump
    {
        get
        {
            if (_player != null)
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
            if (_player != null)
            {
                return _player.GetButton("Build");
            }
            return false;
        }
    }

    public bool ToggleBuildMode
    {
        get
        {
            if (_player != null)
            {
                return _player.GetButton("ToggleBuildingMode");
            }
            return false;
        }
    }

}
