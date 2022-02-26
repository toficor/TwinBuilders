using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControllerData _playerControllerData;
    [SerializeField] private PlayerEquipmentData _playerEquipmentData;
    [SerializeField] private PlayerMovementData _playerMovementData;
    [SerializeField] private PlayerBuildingControllerData _playerBuildingControllerData;
    [SerializeField] private bool _imFirstPlayer;

    private PlayerInput _playerInput;
    private PlayerMotorController _playerMotorController;
    private PlayerBuildingController _playerBuildingController;
    private PlayerAudioController _playerAudioController;
    private PlayerEquipmentController _playerEquipmentController;
    private Collider2D _playerCollider;

    private List<BaseController> _playersBaseControllers = new List<BaseController>();

    public PlayerBuildingController PlayerBuildingController => _playerBuildingController;
    public bool ImFirstPlayer => _imFirstPlayer;

    private void Awake()
    {
        Init();
        _playerCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        _playersBaseControllers.ForEach(x => x.Update());
        if (_playerBuildingController.InBuildingMode || _playerBuildingController.WaitingForSecondPlayer)
        {
            _playerMotorController.DisableMove();
        }
        else
        {
            _playerMotorController.EnableMove();
        }
    }

    private void FixedUpdate()
    {
        _playersBaseControllers.ForEach(x => x.FixedUpdate());
    }

    private void Init()
    {
        _playerInput = new PlayerInput(_playerControllerData);
        _playerMotorController = new PlayerMotorController(GetComponent<Rigidbody2D>(), _playerMovementData, _playerInput, this, this);
        _playerEquipmentController = new PlayerEquipmentController(_playerInput, _playerEquipmentData, this);
        _playerBuildingController = new PlayerBuildingController(_playerInput, _playerEquipmentController, _playerBuildingControllerData, _playerMotorController, this.transform, this);
        _playerAudioController = GetComponent<PlayerAudioController>();

        _playerAudioController?.Init(_playerMotorController);
        _playerMotorController.Init();
        _playerBuildingController.Init();
        _playerEquipmentController.Init();

        _playersBaseControllers.Add(_playerMotorController);
        _playersBaseControllers.Add(_playerBuildingController);
        _playersBaseControllers.Add(_playerEquipmentController);
    }

    private void OnEnable()
    {
        _playerMotorController.Activate();
        _playerBuildingController.Activate();
    }
    private void OnDisable()
    {

    }

}
