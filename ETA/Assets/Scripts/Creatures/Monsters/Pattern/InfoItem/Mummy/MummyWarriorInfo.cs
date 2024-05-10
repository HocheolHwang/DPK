using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// 1. MeleeAttack: ATK
/// 2. WindMill: [ ATK + P_ATK(10) ] * 2번
/// </summary>
public class MummyWarriorInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        PatternList.Add(gameObject.GetOrAddComponent<MummyWarriorMeleeAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyWarriorWindMill>());
    }
}
