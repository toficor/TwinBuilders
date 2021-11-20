using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public abstract class GameSystem : MonoBehaviour
{
    public virtual bool IsReady { get; protected set; }

    public virtual async Task<InitializationResult> Init()
    {
        IsReady = true;
        return InitializationResult.Success;
    }

    public virtual void DeInit()
    {
        IsReady = false;
    }
}

