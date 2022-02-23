using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D _rb;

    [Header("Movement")]
    [SerializeField] private float _movementAccelaration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _linearDrag;
    //this shoudl be fixed
    private bool _canMove = true;

    [Header("Jump")]
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _jumpDelay = 0.15f;
    [SerializeField] private float _extraJump = 1f;
    [SerializeField] private float _hangTime = 0.1f;

    private float _jumpCounter;
    private float _hangTimeTimer;

    private bool _canJump => Input.GetButtonDown("Jump") && (_hangTimeTimer > 0 || _jumpCounter > 0 || _onWall);
    private bool _isJumping = false;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _wallLayer;

    [Header("Collisions")]
    [SerializeField] private float _groundRaycastLength;
    private bool _onGround;
    private bool _leftRayGroundHitResult;
    private bool _rightRayGroundHitResult;
    [SerializeField] private Vector3 _groundVectorsOffset = Vector3.zero;

    [Header("Corner Correction Variables")]
    [SerializeField] private float _topRaycastLength;
    [SerializeField] private Vector3 _edgeRaycastOffset;
    [SerializeField] private Vector3 _innerRaycastOffset;
    private bool _canCornerCorrect;

    [Header("Wall collsions Variables")]
    [SerializeField] private float _wallRaycastLength;
    public bool _onWall;
    public bool _onRightWall;

    [Header("Wall Movement Variables")]
    [SerializeField] private float _wallSlideModifier = 0.5f;
    [SerializeField] private float _wallJumpXVelocityHaltDelay = 0.2f;
    private bool _wallSlide => _onWall && !_onGround && _rb.velocity.y < 0f;

    //wall grab need to be changed to shelf grab or something like this, there is no need to implement wallGrab
    // private bool _wallGrab => _onWall && !_onGround;

    private float _horizontalDirection;
    private bool _changingDirection => (_rb.velocity.x > 0f && _horizontalDirection < 0f) || (_rb.velocity.x < 0f && _horizontalDirection > 0f);




    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _horizontalDirection = GetInput().x;

        if (_canJump)
        {
            if (_onWall && !_onGround)
            {
                if (_onRightWall && _horizontalDirection > 0f || !_onRightWall && _horizontalDirection < 0f)
                {
                    StartCoroutine(NeutralWallJump());
                }
                else
                {
                    WallJump();
                }
            }
            else
            {
                Jump(Vector2.up);
            }

        }

        //  Debug.LogError("On ground: " + _onGround);
    }

    private void FixedUpdate()
    {
        CheckCollisions();
        if (_canMove)
        {
            MoveCharacter();
        }
        else
        {
            //tu chyba jaks chala sie okaze 
            _rb.velocity = Vector2.Lerp(_rb.velocity, (new Vector2(_horizontalDirection * _maxMoveSpeed, _rb.velocity.y)), 0.5f * Time.deltaTime);
        }

        if (_onGround)
        {
            _jumpCounter = _extraJump;
            _hangTimeTimer = _hangTime;
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
        //if (_wallGrab)
        //{
        //    WallGrab();
        //}
    }

    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void MoveCharacter()
    {
        _rb.AddForce(new Vector2(_horizontalDirection, 0f) * _movementAccelaration);
        if (Mathf.Abs(_rb.velocity.x) > _maxMoveSpeed)
        {
            _rb.velocity = new Vector2(Mathf.Sign(_rb.velocity.x) * _maxMoveSpeed, _rb.velocity.y);
        }
    }

    private void ApplyGroundLinearDrag()
    {
        if (Mathf.Abs(_horizontalDirection) < 0.4f || _changingDirection)
        {
            _rb.drag = _linearDrag;
        }
        else
        {
            _rb.drag = 0f;
        }
    }
    private void ApplyAirLinearDrag()
    {
        _rb.drag = _airLinearDrag;
    }

    private void WallSlide()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, -_maxMoveSpeed * _wallSlideModifier);
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

        _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        _rb.AddForce(direction * _jumpForce, ForceMode2D.Impulse);
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
        yield return new WaitForSeconds(_wallJumpXVelocityHaltDelay);
        _rb.velocity = new Vector2(0f, _rb.velocity.y);

    }
    private void CheckCollisions()
    {
        _leftRayGroundHitResult = Physics2D.Raycast(transform.position + _groundVectorsOffset, Vector2.down, _groundRaycastLength, _groundLayer);
        _rightRayGroundHitResult = Physics2D.Raycast(transform.position - _groundVectorsOffset, Vector2.down, _groundRaycastLength, _groundLayer);
        _onGround = _leftRayGroundHitResult || _rightRayGroundHitResult;

        _canCornerCorrect = Physics2D.Raycast(transform.position + _edgeRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) &&
        !Physics2D.Raycast(transform.position + _innerRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) ||
        Physics2D.Raycast(transform.position - _edgeRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer) &&
        !Physics2D.Raycast(transform.position - _innerRaycastOffset, Vector2.up, _topRaycastLength, _groundLayer);

        _onWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer) || Physics2D.Raycast(transform.position, Vector2.left, _wallRaycastLength, _wallLayer);
        _onRightWall = Physics2D.Raycast(transform.position, Vector2.right, _wallRaycastLength, _wallLayer);
    }

    private void CornerCorrect(float velocityY)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.left, _topRaycastLength, _groundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x + newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, velocityY);
            return;
        }

        hit = Physics2D.Raycast(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength, Vector3.right, _topRaycastLength, _groundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * _topRaycastLength, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
            transform.position = new Vector3(transform.position.x - newPos, transform.position.y, transform.position.z);
            _rb.velocity = new Vector2(_rb.velocity.x, velocityY);
        }
    }

    private void FallMultiplier()
    {
        if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _fallMultiplier;
        }
        else if (_rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _rb.gravityScale = _lowJumpFallMultiplier;
        }
        else
        {
            _rb.gravityScale = 1f;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + _groundVectorsOffset, transform.position + _groundVectorsOffset + Vector3.down * _groundRaycastLength);
        Gizmos.DrawLine(transform.position - _groundVectorsOffset, transform.position - _groundVectorsOffset + Vector3.down * _groundRaycastLength);

        Gizmos.DrawLine(transform.position + _edgeRaycastOffset, transform.position + _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _edgeRaycastOffset, transform.position - _edgeRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength);
        Gizmos.DrawLine(transform.position - _innerRaycastOffset, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength);

        Gizmos.DrawLine(transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength, transform.position - _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.left * _topRaycastLength);
        Gizmos.DrawLine(transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength, transform.position + _innerRaycastOffset + Vector3.up * _topRaycastLength + Vector3.right * _topRaycastLength);

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * _wallRaycastLength);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * _wallRaycastLength);
    }
}
