using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerData", menuName = "Data/Player/PlayerControllerData", order = 1)]
public class PlayerControllerData : ScriptableObject
{
    public int RewiredPlayerId = 0;
    public string InputLayoutName = "Default";

    public bool AirControl = true;    

    //this should be private
    public bool FacingRight;
    public bool Grounded;

    public float MaxSpeed = 10;
    public float JumpForce = 800f;
    public float GroundedRadius = .2f;
    public float CrouchSpeed = 0.25f;

    public LayerMask WhatIsGround;   

}
