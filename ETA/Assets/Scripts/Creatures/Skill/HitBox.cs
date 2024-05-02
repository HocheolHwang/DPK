using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    private Transform _attacker;
    private int _damage;
    private int _penetration;           // 관통 부여
    private bool _isCounter;
    private float _duration;

    public int Penetration { get => _penetration; private set => _penetration = value; }

    public void SetUp(Transform attacker, int damage, int penetration = -1, bool isCounter = false, float duration = 0.1f)
    {
        _attacker = attacker;
        _damage = damage;
        _penetration = penetration;
        _isCounter = isCounter;
        _duration = duration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_attacker.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Monster"))
        {
            other.GetComponent<IDamageable>().TakeDamage(_damage, _isCounter);
            _penetration--;
        }
        else if (_attacker.gameObject.CompareTag("Monster") && other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"player name : {other.gameObject.name}");
            other.GetComponent<IDamageable>().TakeDamage(_damage);
            _penetration--;
        }

        // 음수일 때는 계속 관통
        // 0일 때는 비활성화 -> 파괴는 각 클래스에서 따로 처리한다.
        // 양수인 경우도 계속 관통
        if (_penetration == 0)
        {
            gameObject.SetActive(false);
        }
        StartCoroutine(InActiveColider(_duration));
    }

    IEnumerator InActiveColider(float duration)
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
