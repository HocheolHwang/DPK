using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// 1. FirstAuto: ATK + P_ATK(5)  ( 내려찍기 )
/// 2. SecondAuto: ATK     ( 올려치기 )
/// 3. TwoSkill: ATK + P_ATK(80)
/// 4. CounterATK: ATK + P_ATK(130
/// 5. phase ATK: ATK + P_ATK(0) ( 찍기 ) | ATK - 10 ( 퍼지기 )
/// </summary>
public class KnightGInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<KnightGFirstAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<KnightGSecondAutoAttack>());
        // counter
        PatternList.Add(gameObject.GetOrAddComponent<KnightGCounterEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<KnightGCounterAttackPattern>());

        // skill2
        PatternList.Add(gameObject.GetOrAddComponent<KnightGTwoSkillEnergy>());
        PatternList.Add(gameObject.GetOrAddComponent<KnightGTwoSkillAttack>());


        // phase 2
        // phase 2 진입에 성공하면 자식 객체로 아우라 이펙트를 넣고 파괴하지 않는다.
        PatternList.Add(gameObject.GetOrAddComponent<KnightGPhaseTransition>());
        PatternList.Add(gameObject.GetOrAddComponent<KnightGPhaseAttack>());
    }
}
