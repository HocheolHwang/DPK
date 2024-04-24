using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Damage를 받는 Controller에 부착
public interface IDamageable
{
    void TakeDamage(int attackDamage);
    // 피격 이벤트 추가
    void DestroyEvent();
    void DestroyObject();
}
