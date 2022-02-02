using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController
{
    protected bool _isActive;

    public bool IsActive => _isActive;

    public abstract BaseController Init();
    public abstract void DeInit();
    public virtual void Activate()
    {
        _isActive = true;
    }
    public void Update()
    {
        if (_isActive)
        {
            OnUpdate();
        }
    }

    protected abstract void OnUpdate();
    public virtual void DeActivate()
    {
        _isActive = false;
    }
}
