using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage를 받는 Controller에 부착
public interface IDamageable
{
    void TakeDamage(int damage);
    void DestroyEvent();
    void DestroyObject();
}
