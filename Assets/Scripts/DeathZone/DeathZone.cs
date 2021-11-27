using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private PlayerController _myPlayerController;
    private DeathZoneData _myDeathZoneData;
    private Camera _camera;
    private bool _isMoving = false;
    private BoxCollider2D _myCollider;

    private Plane[] _planes;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }

        set
        {
            _isMoving = value;
        }
    }

    public void Init(PlayerController playerController, DeathZoneData deathZoneData, Camera camera)
    {
        _myPlayerController = playerController;
        _myDeathZoneData = deathZoneData;
        _camera = camera;

        _myCollider = GetComponent<BoxCollider2D>();
        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
    }

    private void Update()
    {
        if (IsMoving)
        {
            MoveDeathZone();
        }
    }

    private void MoveDeathZone()
    {
        transform.position += new Vector3(0, _myDeathZoneData.DeathZoneSpeed * Time.deltaTime, 0f);
    }

    public bool IsVisible()
    {
        _planes = GeometryUtility.CalculateFrustumPlanes(_camera);
        if (GeometryUtility.TestPlanesAABB(_planes, _myCollider.bounds))
        {
            return true;
        }
        return false;
    }
}
