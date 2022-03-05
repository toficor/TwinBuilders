using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControllerData _playerControllerData;
    [SerializeField] private PlayerEquipmentData _playerEquipmentData;
    [SerializeField] private PlayerMovementData _playerMovementData;
    [SerializeField] private PlayerBuildingControllerData _playerBuildingControllerData;
    [SerializeField] private bool _imFirstPlayer;

    //debug
    // [SerializeField] private SpriteShapeController _spriteShapeController;

    private PlayerInput _playerInput;
    private PlayerMotorController _playerMotorController;
    private PlayerBuildingController _playerBuildingController;
    private PlayerAudioController _playerAudioController;
    private PlayerEquipmentController _playerEquipmentController;
    private PlayerStateController _playerStateController;
    private PlayerController _cOOPlayerReference;

    private Collider2D _playerCollider;
    private Rigidbody2D _playerRigidbody;
    private SpriteRenderer _playerSpriteRenderer;
    private Animator _playerStateMachine;

    private List<BaseController> _playersBaseControllers = new List<BaseController>();

    public PlayerBuildingController PlayerBuildingController => _playerBuildingController;
    public PlayerMotorController PlayerMotorController => _playerMotorController;
    public PlayerStateController PlayerStateController => _playerStateController;
    public Rigidbody2D PlayerRigidbody => _playerRigidbody;
    public bool ImFirstPlayer => _imFirstPlayer;
    public PlayerBuildingControllerData PlayerBuildingControllerData  => _playerBuildingControllerData;
    public PlayerController COOPlayerReference => _imFirstPlayer ? GameManager.Instance.SecondPlayerReference : GameManager.Instance.FirstPlayerReference;


    private void Awake()
    {
        Init();
        _playerCollider = GetComponent<Collider2D>();
        _playerRigidbody = GetComponent<Rigidbody2D>();
        _playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _playersBaseControllers.ForEach(x => x.Update());

        //to przeniesc to player state controller
        if (_playerStateController.InBuildingMode || _playerStateController.WaitingForSecondPlayer)
        {
            _playerMotorController.DisableMove();
        }
        else
        {
            _playerMotorController.EnableMove();
        }
        //  SpriteShapeTest();
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
        _playerBuildingController.OnPlayerChangeShape += OnPlayerShapeChanged;
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

    private void OnPlayerShapeChanged(PlayerShapeData newPlayerShape)
    {
        _playerSpriteRenderer.sprite = newPlayerShape.Sprite;
        transform.localScale = new Vector3(newPlayerShape.Width, newPlayerShape.Height, 1f);
        //jak to zadziala to niewiem
        gameObject.layer = (int)Mathf.Log(newPlayerShape.LayerMask, 2);
        //zadzialalo xDDDD
        Destroy(_playerCollider);
        _playerCollider = gameObject.AddComponent<PolygonCollider2D>();
    }

    //for now this will be returning position on a top of players head
    public Vector2 GetNearestAvailablePosition()
    {
        return new Vector2(transform.position.x, transform.position.y + _playerCollider.bounds.size.y);
    }

    //debug
    //private void SpriteShapeTest()
    //{
    //    if (_spriteShapeController != null)
    //    {
    //        _spriteShapeController.spline.Clear();
    //        _spriteShapeController.spline.InsertPointAt(0, GameManager.Instance.FirstPlayerReference.transform.position - _spriteShapeController.transform.position);
    //        _spriteShapeController.spline.InsertPointAt(1, GameManager.Instance.SecondPlayerReference.transform.position - _spriteShapeController.transform.position);
    //        _spriteShapeController.spline.SetPosition(0, GameManager.Instance.FirstPlayerReference.transform.position - _spriteShapeController.transform.position);
    //        _spriteShapeController.spline.SetPosition(1, GameManager.Instance.SecondPlayerReference.transform.position - _spriteShapeController.transform.position);
    //    }
    //}

}
