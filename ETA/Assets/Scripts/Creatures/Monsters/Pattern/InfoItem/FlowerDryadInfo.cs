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
public class FlowerDryadInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<FlowerDryadFirstAutoAttack>());
        // counter
        PatternList.Add(gameObject.GetOrAddComponent<FlowerDryadCounterEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<FlowerDryadCounterAttackPattern>());

    }
}
