using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    private Transform _attacker;
    private int _damage;
    private int _penetration;           // 관통 부여

    public void SetUp(Transform attacker, int damage, int penetration = -1)
    {
        _attacker = attacker;
        _damage = damage;
        _penetration = penetration;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        if (_attacker.gameObject.CompareTag("Player") && other.gameObject.CompareTag("Monster"))
        {
            other.GetComponent<IDamageable>().TakeDamage(_damage);
            _penetration--;
        }
        else if (_attacker.gameObject.CompareTag("Monster") && other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<IDamageable>().TakeDamage(_damage);
            _penetration--;
        }

        // 음수일 때는 계속 관통
        // 0일 때는 파괴
        // 양수인 경우도 계속 관통
        if (_penetration == 0)
        {
            Destroy(gameObject);
        }
    }
}
