using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Data/Player/PlayerMovementData", order = 1)]
public class PlayerMovementData : ScriptableObject
{
    [Header("Movement Horizontal")]
    [SerializeField] private float _movementAccelaration;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _linearDrag;
    public float MovementAccelaration => _movementAccelaration;
    public float MaxMoveSpeed => _maxMoveSpeed;
    public float LinearDrag => _linearDrag;

    [Header("Jump")]
    [SerializeField] private float _airLinearDrag = 2.5f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _fallMultiplier = 8f;
    [SerializeField] private float _lowJumpFallMultiplier = 5f;
    [SerializeField] private float _jumpDelay = 0.15f;
    [SerializeField] private int _extraJump = 1;
    [SerializeField] private float _hangTime = 0.1f;

    public float AirLinearDrag => _airLinearDrag;
    public float JumpForce => _jumpForce;
    public float FallMultiplier => _fallMultiplier;
    public float LowJumpFallMultiplier => _lowJumpFallMultiplier;
    public float JumpDelay => _jumpDelay;
    public int ExtraJump => _extraJump;
    public float HangTime => _hangTime;

    //TODO: Made every as properties
    [Header("Layer Masks")]
    public LayerMask GroundLayer;
    //[SerializeField] public LayerMask _groundLayer;
    public LayerMask WallLayer;
   // [SerializeField] public LayerMask _wallLayer;

    [Header("Collisions")]
    [SerializeField] public float _groundRaycastLength;
    [SerializeField] public Vector3 _groundVectorsOffset = Vector3.zero;


    [Header("Corner Correction Variables")]
    [SerializeField] public float _topRaycastLength;
    [SerializeField] public Vector3 _edgeRaycastOffset;
    [SerializeField] public Vector3 _innerRaycastOffset;


    [Header("Wall collsions Variables")]
    [SerializeField] public float _wallRaycastLength;


    [Header("Wall Movement Variables")]
    [SerializeField] public float _wallSlideModifier = 0.5f;
    [SerializeField] public float _wallJumpXVelocityHaltDelay = 0.2f;
}
