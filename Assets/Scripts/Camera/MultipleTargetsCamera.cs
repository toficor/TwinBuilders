using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetsCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> _targets;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothTime = .5f;
    [SerializeField] private float _zoomLimiter = 50f;
    [SerializeField] private Vector2 _minMaxZoom;

    private Vector3 _velocity;
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (_targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(_minMaxZoom.x, _minMaxZoom.y, GetGreatestDistance() / _zoomLimiter);
        _camera.orthographicSize = newZoom;
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + _offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            bounds.Encapsulate(_targets[i].position);
        }

        return bounds.size.x;
    }

    private Vector3 GetCenterPoint()
    {
        if (_targets.Count == 1)
        {
            return _targets[0].position;
        }

        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            bounds.Encapsulate(_targets[i].position);
        }

        return bounds.center;
    }
}
