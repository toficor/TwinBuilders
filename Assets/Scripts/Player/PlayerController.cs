using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControllerData _playerControllerData;
    [SerializeField] private PlayerEquipmentData _playerEquipmentData;
    [SerializeField] private Transform _groundCheckTransform;
    [SerializeField] private LayerMask _groundLayers;

    private PlayerInput _playerInput;
    private PlayerMotorController _playerMotorController;
    private PlayerBuildingController _playerBuildingController;
    private PlayerAudioController _playerAudioController;
    private PlayerEquipmentController _playerEquipmentController;
    private Collider2D _playerCollider;

    private List<BaseController> _playersBaseControllers = new List<BaseController>();

    private void Awake()
    {
        Init();
        _playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _playersBaseControllers.ForEach(x => x.Update());
        GroundCheck();
    }

    private void FixedUpdate()
    {
        _playersBaseControllers.ForEach(x => x.FixedUpdate());
    }

    private void Init()
    {
        _playerInput = new PlayerInput(_playerControllerData);
        _playerMotorController = new PlayerMotorController(GetComponent<Animator>(), GetComponent<Rigidbody2D>(), _playerControllerData, _playerInput, gameObject);
        _playerEquipmentController = new PlayerEquipmentController(_playerInput, _playerEquipmentData);
        _playerBuildingController = new PlayerBuildingController(_playerInput, _playerEquipmentController);
        _playerAudioController = GetComponent<PlayerAudioController>();

        _playerAudioController.Init(_playerMotorController);
        _playerMotorController.Init();
        _playerBuildingController.Init();
        _playerEquipmentController.Init();       

        _playersBaseControllers.Add(_playerMotorController);
        _playersBaseControllers.Add(_playerBuildingController);
        _playersBaseControllers.Add(_playerEquipmentController);
    }

    private void GroundCheck()
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(_playerCollider.bounds.center, -_playerCollider.transform.up, _playerCollider.bounds.extents.y + 0.02f, _groundLayers);
        _playerControllerData.Grounded = raycastHit2D.collider != null;
    }
    private void OnEnable()
    {
        _playerMotorController.Activate();
    }
    private void OnDisable()
    {

    }

}
