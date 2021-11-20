using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControllerData _playerControllerData;
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private LayerMask _groundLayers;

    private PlayerInput _playerInput;
    private PlayerMotor _playerMotor;
    private PlayerBuildingController _playerBuildingController;
    private PlayerAudioController _playerAudioController;
    private BoxCollider2D _playerBoxCollider;

    private void Awake()
    {
        Init();
        _playerBoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        _playerMotor.MotorUpdate();
        GroundCheck();
    }

    private void FixedUpdate()
    {
        _playerMotor.MotorFixedUpdate();
    }

    private void Init()
    {
        _playerInput = new PlayerInput(_playerControllerData);
        _playerMotor = new PlayerMotor(GetComponent<Animator>(), GetComponent<Rigidbody2D>(), _playerControllerData, _playerInput, gameObject);
        _playerAudioController = GetComponent<PlayerAudioController>();
        _playerAudioController.Init(_playerMotor);
    }

    private void GroundCheck()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(_playerBoxCollider.bounds.center, -_playerBoxCollider.transform.up, _playerBoxCollider.bounds.extents.y + 0.02f, _groundLayers);
        _playerControllerData.Grounded = raycastHit2D.collider != null;
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {

    }

}
