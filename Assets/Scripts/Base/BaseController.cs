using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
    protected bool _isActive;

    public bool IsActive => _isActive;

    public abstract void Init();
    public abstract void DeInit();
    public virtual void Activate()
    {
        _isActive = true;
    }

    public virtual void DeActivate()
    {
        _isActive = false;
    }
}
