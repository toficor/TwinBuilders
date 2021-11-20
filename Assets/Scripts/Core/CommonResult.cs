using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonResult
{
    public static CommonResult Success
    {
        get
        {
            return new CommonResult();
        }
    }

    public static CommonResult Error(string errorMsg)
    {
        Debug.LogError(errorMsg);

        return new CommonResult { ErrorMessage = errorMsg };
    }

    public string ErrorMessage { get; protected set; }

    public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);
    public bool IsError => !string.IsNullOrEmpty(ErrorMessage);
}


