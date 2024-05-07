using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuffBox : MonoBehaviour
{

    private Transform _caster;
    private int _amount;
    private float _duration;
    // private stat _stat;
    // public enum stat { Hp, Shield, Defense };

    public void SetUp(Transform caster, int amount, float duration = 1.0f)
    {
        _caster = caster;
        _amount = amount;
        // _stat = stat;
        _duration = duration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_caster.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<IBuffStat>().GetShield(_amount);
        }

        StartCoroutine(InActiveColider(_duration));
    }

    // private void ApplyBuff(BaseController _controller)
    // {
    //     switch (_stat)
    //     {
    //         case stat.Hp:
    //             // _controller.IncreaseHP(_amount);
    //             break;
    //         case stat.Shield:
    //             _controller.GetShield(_amount);
    //             break;
    //         case stat.Defense:
    //             // _controller.IncreaseDefense(_amount);
    //             break;
    //     }
    // }

    IEnumerator InActiveColider(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
