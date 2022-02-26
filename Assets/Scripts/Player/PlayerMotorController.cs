using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMotorController : BaseController
{

    private Rigidbody2D _rb;
    private PlayerMovementData _playerMovementData;
    private PlayerInput _playerInput;
    private Transform _playerTransform;
    private MonoBehaviour _mainControllerMonoBehaviourInstance;
    //this shoudl be fixed
    private bool _canMove = true;

    private float _jumpCounter;
    private float _hangTimeTimer;

    private bool _canJump => _playerInput.Jump && (_hangTimeTimer > 0 || _jumpCounter > 0 || _onWall);
    private bool _isJumping = false;

    private bool _onGround;
    private bool _leftRayGroundHitResult;
    private bool _rightRayGroundHitResult;

    private bool _canCornerCorrect;

    public bool _onWall;
    public bool _onRightWall;

    private bool _wallSlide => _onWall && !_onGround && _rb.velocity.y < 0f;

    //wall grab need to be changed to shelf grab or something like this, there is no need to implement wallGrab
    // private bool _wallGrab => _onWall && !_onGround;

    private float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);

    public bool OnGround => _onGround;
    public bool CanMove => _canMove;

    public PlayerMotorController(Rigidbody2D rigidbody,
                                 PlayerMovementData playerMovementData,
                                 PlayerInput playerInput,
                                 MonoBehaviour monoBehaviour,
                                 PlayerController playerController) : base(playerController)
    {
        _rb = rigidbody;
        _playerMovementData = playerMovementData;
        _playerTransform = monoBehaviour.transform;
        _playerInput = playerInput;
        _mainControllerMonoBehaviourInstance = monoBehaviour;
    }


    public override void DeInit()
    {
        throw new NotImplementedException();
    }

    public override BaseController Init()
    {
        return this;
    }

    protected override void OnFixedUpdate()
    {

        CheckCollisions();
        if (_canMove)
        {
            MoveCharacter();
        }
        else
        {
            //tu chyba jaks chala sie okaze 
            // _rb.velocity = Vector2.Lerp(_rb.velocity, (new Vector2(_horizontalDirection * _playerMovementData.MaxMoveSpeed, _rb.velocity.y)), 0.5f * Time.deltaTime);
            _rb.velocity = Vector2.zero;
        }

        if (_onGround)
        {
            _jumpCounter = _playerMovementData.ExtraJump;
            _hangTimeTimer = _playerMovementData.HangTime;
            ApplyGroundLinearDrag();
        }
        else
        {
            _hangTimeTimer -= Time.fixedDeltaTime;
            ApplyAirLinearDrag();
            FallMultiplier();
            if (!_onWall || _rb.velocity.y < 0f)
            {
                _isJumping = false;
            }
        }
        if (_canCornerCorrect)
        {
            CornerCorrect(_rb.velocity.y);
        }

        if (!_isJumping)
        {
            if (_wallSlide)
            {
                WallSlide();
            }
        }
    }

    int debugFrameCounter = 0;
    protected override void OnUpdate()
    {
        // DebugDrawLine();

        _horizontalDirection = GetInput().x;

        if (_canJump)
        {
            if (_onWall && !_onGround)
            {
                if (_onRightWall && _horizontalDirection > 0f || !_onRightWall && _horizontalDirection < 0f)
                {
                    _mainControllerMonoBehaviourInstance.StartCoroutine(NeutralWallJump());
                }
                else
                {
                    WallJump();
                }
            }
            else
            {
                // Debug.LogError(debugFrameCounter++);
                Jump(Vector2.up);
            }

        }
        else
        {
            debugFrameCounter = 0;
        }

        //  Debug.LogError("On ground: " + _onGround);
    }
    public void DisableMove()
    {
        _canMove = false;
    }

    public void EnableMove()
    {
        _canMove = true;
    }
    private Vector2 GetInput()
    {
        return new Vector2(_playerInput.Horizontal, 0f);
    }
    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _playerMovementData.MovementAccelaration);
        if (Mathf.Abs(_rb.velocity.x) > _playerMovementData.MaxMoveSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _playerMovementData.MaxMoveSpeed, _rb.velocity.y);
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _playerMovementData.LinearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }
    private void ApplyAirLinearDrag()
    {
        _rb.drag = _playerMovementData.AirLinearDrag;
    }

    private void WallSlide()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -_playerMovementData.MaxMoveSpeed * _playerMovementData._wallSlideModifier);
        StickToWall();
    }

    //private void WallGrab()
    //{
    //    _rb.gravityScale = 0f;
    //    _rb.velocity = new Vector2(_rb.velocity.x, 0f);
    //  StickToWall();
    //}

    private void StickToWall()
    {
        if (_onRightWall && _horizontalDirection >= 0f)
        {
            _rb.velocity = new Vector2(1, _rb.velocity.y);
        }
        else if (!_onRightWall && _horizontalDirection <= 0f)
        {
            _rb.velocity = new Vector2(-1, _rb.velocity.y);
        }
    }

    private void Jump(Vector2 direction)
    {
        if (!_onGround)
        {
            _jumpCounter--;
            _hangTimeTimer = 0f;
        }

        // Debug.LogError("skacze");
        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _playerMovementData.JumpForce, ForceMode2D.Impulse);
        _isJumping = true;
    }

    private void WallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }

    private IEnumerator NeutralWallJump()
    {
        Vector2 jumpDirection = _onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(_playerMovementData._wallJumpXVelocityHaltDelay);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);

    }
    private void CheckCollisions()
    {
        _leftRayGroundHitResult = Physics2D.Raycast(_playerTransform.position + _playerMovementData._groundVectorsOffset, Vector2.down, _playerMovementData._groundRaycastLength, _playerMovementData.GroundLayer);
        _rightRayGroundHitResult = Physics2D.Raycast(_playerTransform.position - _playerMovementData._groundVectorsOffset, Vector2.down, _playerMovementData._groundRaycastLength, _playerMovementData.GroundLayer);
        _onGround = _leftRayGroundHitResult || _rightRayGroundHitResult;

        _canCornerCorrect = Physics2D.Raycast(_playerTransform.position + _playerMovementData._edgeRaycastOffset, Vector2.up, _playerMovementData._topRaycastLength, _playerMovementData.GroundLayer) &&
        !Physics2D.Raycast(_playerTransform.position + _playerMovementData._innerRaycastOffset, Vector2.up, _playerMovementData._topRaycastLength, _playerMovementData.GroundLayer) ||
        Physics2D.Raycast(_playerTransform.position - _playerMovementData._edgeRaycastOffset, Vector2.up, _playerMovementData._topRaycastLength, _playerMovementData.GroundLayer) &&
        !Physics2D.Raycast(_playerTransform.position - _playerMovementData._innerRaycastOffset, Vector2.up, _playerMovementData._topRaycastLength, _playerMovementData.GroundLayer);

        _onWall = Physics2D.Raycast(_playerTransform.position, Vector2.right, _playerMovementData._wallRaycastLength, _playerMovementData.WallLayer) || Physics2D.Raycast(_playerTransform.position, Vector2.left, _playerMovementData._wallRaycastLength, _playerMovementData.WallLayer);
        _onRightWall = Physics2D.Raycast(_playerTransform.position, Vector2.right, _playerMovementData._wallRaycastLength, _playerMovementData.WallLayer);
    }

    private void CornerCorrect(float velocityY)
    {
        RaycastHit2D hit = Physics2D.Raycast(_playerTransform.position - _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength, Vector3.left, _playerMovementData._topRaycastLength, _playerMovementData.GroundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, _playerTransform.position.y, 0f) + Vector3.up * _playerMovementData._topRaycastLength, _playerTransform.position - _playerMovementData._edgeRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength);
            _playerTransform.position = new Vector3(_playerTransform.position.x + newPos, _playerTransform.position.y, _playerTransform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, velocityY);
            return;
        }

        hit = Physics2D.Raycast(_playerTransform.position + _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength, Vector3.right, _playerMovementData._topRaycastLength, _playerMovementData.GroundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, _playerTransform.position.y, 0f) + Vector3.up * _playerMovementData._topRaycastLength, _playerTransform.position + _playerMovementData._edgeRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength);
            _playerTransform.position = new Vector3(_playerTransform.position.x - newPos, _playerTransform.position.y, _playerTransform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, velocityY);
        }
    }

    private void FallMultiplier()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _playerMovementData.FallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !_playerInput.Jump)
        {
            _rb.gravityScale = _playerMovementData.LowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1f;
        }
    }

    private void DebugDrawLine()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_playerTransform.position + _playerMovementData._groundVectorsOffset, _playerTransform.position + _playerMovementData._groundVectorsOffset + Vector3.down * _playerMovementData._groundRaycastLength);
        Gizmos.DrawLine(_playerTransform.position - _playerMovementData._groundVectorsOffset, _playerTransform.position - _playerMovementData._groundVectorsOffset + Vector3.down * _playerMovementData._groundRaycastLength);

        Gizmos.DrawLine(_playerTransform.position + _playerMovementData._edgeRaycastOffset, _playerTransform.position + _playerMovementData._edgeRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength);
        Gizmos.DrawLine(_playerTransform.position - _playerMovementData._edgeRaycastOffset, _playerTransform.position - _playerMovementData._edgeRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength);
        Gizmos.DrawLine(_playerTransform.position + _playerMovementData._innerRaycastOffset, _playerTransform.position + _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength);
        Gizmos.DrawLine(_playerTransform.position - _playerMovementData._innerRaycastOffset, _playerTransform.position - _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength);

        Gizmos.DrawLine(_playerTransform.position - _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength, _playerTransform.position - _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength + Vector3.left * _playerMovementData._topRaycastLength);
        Gizmos.DrawLine(_playerTransform.position + _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength, _playerTransform.position + _playerMovementData._innerRaycastOffset + Vector3.up * _playerMovementData._topRaycastLength + Vector3.right * _playerMovementData._topRaycastLength);

        Gizmos.DrawLine(_playerTransform.position, _playerTransform.position + Vector3.right * _playerMovementData._wallRaycastLength);
        Gizmos.DrawLine(_playerTransform.position, _playerTransform.position + Vector3.left * _playerMovementData._wallRaycastLength);
    }

}
