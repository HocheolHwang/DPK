using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    private Transform _attacker;
    private int _damage;

    public void SetUp(Transform attacker, int damage)
    {
        _attacker = attacker;
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            other.GetComponent<IDamageable>().TakeDamage(_damage);
        }
        
    }
}
