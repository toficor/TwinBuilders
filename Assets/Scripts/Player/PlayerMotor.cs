using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMotor
{
    public Action OnJump = delegate { };

    private Animator _playerAnimator;
    private bool _grounded;
    private bool _airControl;
    private float _crouchSpeed;
    private Rigidbody2D _playerRigidbody2D;
    private float _maxSpeed;
    private bool _facingRight;
    private float _jumpForce;
    private GameObject _playerGameObject;
    private PlayerControllerData _playerControllerData;
    private PlayerInput _playerInput;


    public PlayerMotor(Animator anim, Rigidbody2D rigidbody2D, PlayerControllerData playerControllerData, PlayerInput playerInput, GameObject playerGameObject)
    {
        _playerAnimator = anim;
        _airControl = playerControllerData.AirControl;
        _crouchSpeed = playerControllerData.CrouchSpeed;
        _playerRigidbody2D = rigidbody2D;
        _maxSpeed = playerControllerData.MaxSpeed;
        _facingRight = playerControllerData.FacingRight;
        _jumpForce = playerControllerData.JumpForce;
        _playerGameObject = playerGameObject;
        _playerControllerData = playerControllerData;
        _playerInput = playerInput;
    }

    public void MotorUpdate()
    {
       
    }

    public void MotorFixedUpdate()
    {
        Move(_playerInput.Horizontal, _playerInput.Jump);
    }  

    private void Move(float move, bool jump, bool crouch = false)
    {
        // Set whether or not the character is crouching in the animator
        _playerAnimator.SetBool("Crouch", crouch);

        //only control the player if grounded or airControl is turned on
        if (_playerControllerData.Grounded || _airControl)
        {
            // Reduce the speed if crouching by the crouchSpeed multiplier
            move = (crouch ? move * _crouchSpeed : move);

            // The Speed animator parameter is set to the absolute value of the horizontal input.
            _playerAnimator.SetFloat("Speed", Mathf.Abs(move));

            // Move the character
            _playerRigidbody2D.velocity = new Vector2(move * _maxSpeed, _playerRigidbody2D.velocity.y);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && _facingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && !_facingRight)
            {
                // ... flip the player.
                Flip();
            }
        }

        if (_playerControllerData.Grounded && jump && _playerAnimator.GetBool("Ground"))
        {
            _playerAnimator.SetBool("Ground", false);
            _playerRigidbody2D.AddForce(new Vector2(0f, _jumpForce));
            OnJump?.Invoke();
        }

        _playerAnimator.SetBool("Ground", _playerControllerData.Grounded);
        _playerAnimator.SetFloat("vSpeed", _playerRigidbody2D.velocity.y);
    }

    private void Flip()
    {
        _facingRight = !_facingRight;

        Vector3 theScale = _playerGameObject.transform.localScale;
        theScale.x *= -1;
        _playerGameObject.transform.localScale = theScale;
    }
}
