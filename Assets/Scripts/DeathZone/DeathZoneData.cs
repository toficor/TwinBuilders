using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathZoneData", menuName = "Data/DeathZone/DeathZoneData", order = 1)]
public class DeathZoneData : ScriptableObject
{
    public float DeathZoneSpeed = 1f;
    public float IndicatorsFlickingSpeed = 1f;
}
