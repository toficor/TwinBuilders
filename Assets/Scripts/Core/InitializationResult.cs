using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationResult : CommonResult
{
    public static InitializationResult Success
    {
        get
        {
            return new InitializationResult();
        }
    }
}
