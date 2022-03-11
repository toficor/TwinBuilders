using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUIController : BaseController
{
    private PlayerStateUIData _playerStateUIData;
    private SpriteRenderer _playerStateUISpriteRenderer;

    public PlayerStateUIController(SpriteRenderer playerStateUISpriteRenderer, PlayerStateUIData playerStateUIData, PlayerController playerController) : base(playerController)
    {
        _playerStateUIData = playerStateUIData;
        _playerStateUISpriteRenderer = playerStateUISpriteRenderer;
    }

    public override BaseController Init()
    {
        PlayerController.PlayerStateController.OnPlayerEnterAnyState += OnPlayerStateChanged;
        _playerStateUISpriteRenderer.color = new Color(1, 1, 1, 0);
        return this;
    }

    public override void DeInit()
    {
        PlayerController.PlayerStateController.OnPlayerEnterAnyState -= OnPlayerStateChanged;
    }

    protected override void OnUpdate()
    {

    }

    protected override void OnFixedUpdate()
    {

    }

    private void OnPlayerStateChanged(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Platforming:
                _playerStateUISpriteRenderer.color = new Color(1, 1, 1, 0);
                break;
            case PlayerState.WaitingForAction:
                _playerStateUISpriteRenderer.color = new Color(1, 1, 1, 1);
                _playerStateUISpriteRenderer.sprite = _playerStateUIData.WaitingForActionSprite;
                break;
            case PlayerState.Building:
                _playerStateUISpriteRenderer.color = new Color(1, 1, 1, 1);
                _playerStateUISpriteRenderer.sprite = _playerStateUIData.BuildingSprite;
                break;
        }
    }
}
