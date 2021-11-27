using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudioController : MonoBehaviour
{
    [EventRef]
    public string BacgroundAudio = "";

    private void OnEnable()
    {
        RuntimeManager.PlayOneShot(BacgroundAudio);
    }
}
