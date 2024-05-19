using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Init -> PatternInfo에서 실행
public interface IPattern
{
    int AttackDamage { get; }               // Pattern 데미지
    Vector3 PatternRange { get; }           // Pattern 범위
    float CreateTime { get; }               // Effect와 Hitbox가 생성되는 시간
    BaseMonsterController Controller { get; }

    void Init();                            // 쿨타임, Component 등 변수 세팅

    // --------------------------- Pattern Logic ------------------------------
    void Cast();                                     // StartPatternCast 실행 - 스킬 실행
    void StopCast();                                 // 멈춤
    abstract IEnumerator StartPatternCast();         // 스킬 로직 구현
}
