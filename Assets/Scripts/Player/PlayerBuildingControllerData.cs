using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBuildingControllerData", menuName = "Data/Player/PlayerBuildingControllerData", order = 1)]
public class PlayerBuildingControllerData : ScriptableObject
{
    [SerializeField] private float _buildingEnableDistance = 2f;
    public float BuildingEnableDistance => _buildingEnableDistance;

    [SerializeField] private float _buildingEnableTimer = 1f;
    public float WaitingForBuildingTimer => _buildingEnableTimer;

    [SerializeField] private LayerMask _cOPlayerLayerMask;
    public LayerMask COOPlayerLayerMask => _cOPlayerLayerMask;

    [SerializeField] private float _timeBetweenChangingShapes = 0.6f;
    public float TimeBetweenChangingShapes => _timeBetweenChangingShapes;
}
