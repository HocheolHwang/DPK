using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// hitbox를 이용해서 counter 처리해본다.
/// </summary>
public class CounterBox : MonoBehaviour
{
    private Transform _attacker;
    private BaseController _controller;
    private int _penetration;
    private float _duration;

    public Action CounterEvent;

    public void SetUp(Transform attacker, BaseController controller, int penetration = 1, float duration = 0.1f)
    {
        _attacker = attacker;
        _controller = controller;
        _penetration = penetration;
        _duration = duration;
        
        StartCoroutine(InActiveColider(_duration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_attacker.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Monster"))
        {

        }

        if (_penetration == 0)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator InActiveColider(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    public void SetActiveCollider()
    {
        gameObject.SetActive(true);
    }
}
