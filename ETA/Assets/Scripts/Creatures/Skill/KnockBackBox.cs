using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnockBackBox : MonoBehaviour
{

    private Transform _attacker;
    private int _power;
    private int _penetration;           // 관통 부여
    private bool _isCounter;
    private float _duration;
    private float _time;

    public int Penetration { get => _penetration; private set => _penetration = value; }

    public void SetUp(Transform attacker, int power, float time, int penetration = -1, float duration = 0.1f)
    {
        _attacker = attacker;
        _power = power;
        _penetration = penetration;
        _duration = duration;
        _time = time;
        StartCoroutine(InActiveColider(_duration));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_attacker.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Monster"))
        {
            other.GetComponent<IDamageable>().Pushed(_power, _time);
            _penetration--;
        }
        else if (_attacker.gameObject.CompareTag("Monster") && other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"push player : {other.gameObject.name}");
            other.GetComponent<IDamageable>().Pushed(_power, _time);
            _penetration--;
        }

        // 음수일 때는 계속 관통
        // 0일 때는 비활성화 -> 파괴는 각 클래스에서 따로 처리한다.
        // 양수인 경우도 계속 관통
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
