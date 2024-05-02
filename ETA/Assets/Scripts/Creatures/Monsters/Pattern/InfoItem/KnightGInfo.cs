using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// pattern DMG( player 레벨에 따라 수치 조정 )
/// 1. FirstAuto: 20 + 5  ( 내려찍기 )
/// 2. SecondAuto: 20     ( 올려치기 )
/// 3. TwoSkill: 20 + 80
/// 4. CounterATK: 20 + 130
/// 5. phase ATK: 20 + 0 ( 찍기 ) | 20 - 10 ( 퍼지기 )
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
