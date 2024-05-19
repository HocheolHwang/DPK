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
    private stat _stat;
    private String _target;
    public enum stat { Hp, Shield, Defense, MoveSpeed, AttackDamage };

    public void SetUp(Transform caster, int amount, stat stat, float duration = 1.0f, String target = "Player")
    {
        _caster = caster;
        _amount = amount;
        _stat = stat;
        _duration = duration;
        _target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_caster.gameObject.CompareTag("Player") && other.gameObject.CompareTag(_target))
        {
            ApplyBuff(other);
        }

        StartCoroutine(InActiveColider(_duration));
    }

    private void ApplyBuff(Collider other)
    {
        switch (_stat)
        {
            case stat.Hp:
                if (_amount > 0)
                    other.GetComponent<IBuffStat>().IncreaseHp(_amount);
                else if (_amount < 0)
                    other.GetComponent<IBuffStat>().DecreaseHp(-_amount);
                break;
            case stat.Shield:
                if (_amount > 0)
                    other.GetComponent<IBuffStat>().GetShield(_amount);
                else if (_amount < 0)
                    other.GetComponent<IBuffStat>().RemoveShield(-_amount);
                break;
            case stat.Defense:
                if (_amount > 0)
                    other.GetComponent<IBuffStat>().IncreaseDefense(_amount);
                else if (_amount < 0)
                    other.GetComponent<IBuffStat>().DecreaseDefense(-_amount);
                break;
            case stat.MoveSpeed:
                if (_amount > 0)
                    other.GetComponent<IBuffStat>().IncreaseSpeed(_amount);
                else if (_amount < 0)
                    other.GetComponent<IBuffStat>().DecreaseSpeed(-_amount);
                break;
        }
    }

    IEnumerator InActiveColider(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
