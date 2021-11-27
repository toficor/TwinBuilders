using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneController : MonoBehaviour
{

    [SerializeField] private DeathZoneData _deathZoneData;
    [SerializeField] private GameObject _deathZonePrefab;
    [SerializeField] private CanvasGroup _deathZoneIndicatorsParent;
    [SerializeField] private Transform _deathZoneSpawn;

    //this should be get with event or smth.
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Camera _playerCamera;

    private DeathZone _myDeathZone;
    private float _indicatorsTimer;

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        ManageIndicatorsVisibility();
    }

    public void Init()
    {
        _myDeathZone = Instantiate(_deathZonePrefab, _deathZoneSpawn.transform.position, Quaternion.identity).GetComponent<DeathZone>();
        _myDeathZone.Init(_playerController, _deathZoneData, _playerCamera);
        _myDeathZone.IsMoving = true;
    }

    public void ManageIndicatorsVisibility()
    {
        if (_myDeathZone != null)
        {
            if (_myDeathZone.IsVisible())
            {
                _deathZoneIndicatorsParent.alpha = 0f;
                _indicatorsTimer = 0f;
            }
            else
            {
                _indicatorsTimer += Time.deltaTime * _deathZoneData.IndicatorsFlickingSpeed;
                _deathZoneIndicatorsParent.alpha = Mathf.Lerp(0, 1, Mathf.PingPong(_indicatorsTimer, 1));
            }
        }
    }


}
