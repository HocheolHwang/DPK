using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CoroutineManager:MonoBehaviour
{
    public void Init(Managers parent)
    {
    }

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
