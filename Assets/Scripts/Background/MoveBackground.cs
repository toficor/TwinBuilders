using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _offset;
    private Vector2 _startPosition;
    private float _nextYPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        _nextYPosition = Mathf.Repeat(Time.time * -_moveSpeed, _offset);
        transform.position = _startPosition + Vector2.up * _nextYPosition;
    }
}
