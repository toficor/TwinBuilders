using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: I've made IBaseController interface - figure out if it's necessaryowo 
public abstract class BaseController
{
    protected bool _isActive;
    private PlayerController _playerController;

    public bool IsActive => _isActive;
    protected PlayerController PlayerController => _playerController;

    protected BaseController(PlayerController playerController)
    {
        _playerController = playerController;
    }

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

    public void FixedUpdate()
    {
        if (_isActive)
        {
            OnFixedUpdate();
        }
    }

    protected abstract void OnUpdate();

    protected abstract void OnFixedUpdate();
    public virtual void DeActivate()
    {
        _isActive = false;
    }
}
