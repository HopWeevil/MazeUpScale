using CodeBase.Infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour , ICoroutineRunner
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
