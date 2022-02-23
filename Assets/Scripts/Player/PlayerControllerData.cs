using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControllerData", menuName = "Data/Player/PlayerControllerData", order = 1)]
public class PlayerControllerData : ScriptableObject
{
    public int RewiredPlayerId = 0;
    public string InputLayoutName = "Default";
}
