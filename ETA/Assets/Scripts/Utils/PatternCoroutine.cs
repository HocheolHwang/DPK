using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternCoroutine : MonoBehaviour
{
    private IEnumerator _enumerator;
    private Coroutine _coroutine;

    //[SerializeField] private int buffATK = 10;
    //[SerializeField] private int buffDEF = 5;

    //public Action<Transform, int, int> CheckBuf;

    private void Update()
    {
        if (_enumerator != null && _coroutine == null)
        {
            _coroutine = StartCoroutine(_enumerator);
        }
    }

    //private void OnDestroy()
    //{
    //    CheckBuf?.Invoke(transform, buffATK, buffDEF);
    //}

    public IEnumerator Enumerator
    {
        get => _enumerator;
        set => _enumerator = value;
    }
}
