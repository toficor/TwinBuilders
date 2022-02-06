using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseController 
{
    BaseController Init();
    void DeInit();
    void Activate();
    void DeActivate();
    void Update();
    void FixedUpdate();
    void OnUpdate();
    void OnFixedUpdate();
}
