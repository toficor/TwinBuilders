using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStateUIData", menuName = "Data/Player/PlayerStateUIData", order = 1)]
public class PlayerStateUIData : ScriptableObject
{
    [field: SerializeField] public Sprite WaitingForActionSprite { get; private set; }
    [field: SerializeField] public Sprite BuildingSprite { get; private set; }
}
