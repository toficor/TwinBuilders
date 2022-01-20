using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour
{
    [SerializeField] private Transform[] _backgrounds;
    [SerializeField] private Transform _cameraTransfrom;
    private float[] _parallaxScales;

    [SerializeField] private float _smoothing = 1f;
    private Vector3 _previousCamPosition;

    private void Start()
    {
        _previousCamPosition = _cameraTransfrom.position;
        _parallaxScales = new float[_backgrounds.Length];

        for(int i = 0; i < _backgrounds.Length; i++)
        {
            _parallaxScales[i] = _backgrounds[i].position.z;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _backgrounds.Length; i++)
        {
            float parallax = (_previousCamPosition.y - _cameraTransfrom.position.y) * _parallaxScales[i];
            float backTargetPosY = _backgrounds[i].position.y + parallax;
            Vector3 backgroumdTargetPos = new Vector3(_backgrounds[i].position.x, backTargetPosY, _backgrounds[i].position.z);

            _backgrounds[i].position = Vector3.Lerp(_backgrounds[i].position, backgroumdTargetPos, _smoothing * Time.deltaTime);
        }

        _previousCamPosition = _cameraTransfrom.position;
    }
}
