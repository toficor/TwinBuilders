using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController _firstPlayerReference;
    [SerializeField] private PlayerController _secondPlayerReference;

    public PlayerController FirstPlayerReference => _firstPlayerReference;
    public PlayerController SecondPlayerReference => _secondPlayerReference;
}
