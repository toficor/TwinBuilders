using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControllerData _playerControllerData;

    private PlayerInput _playerInput;
    private PlayerMotor _playerMotor;
    private PlayerBuildingController _playerBuildingController;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        _playerMotor.MotorUpdate();
    }

    private void FixedUpdate()
    {
        _playerMotor.MotorFixedUpdate();
    }

    private void Init()
    {
        _playerInput = new PlayerInput(_playerControllerData);
        _playerMotor = new PlayerMotor(GetComponent<Animator>(), GetComponent<Rigidbody2D>(), _playerControllerData, _playerInput,gameObject, transform.Find("GroundCheck").gameObject);   
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

}
