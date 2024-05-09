using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// 1. MeleeAttack: ATK, ATK, [ ATK + P_ATK(20) ] * 2
/// 2. RangedAttack: ATK + P_ATK(5)
/// 3. WindMill: ATK + P_ATK(20)
/// </summary>
public class MummyManInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        PatternList.Add(gameObject.GetOrAddComponent<MummyManRangedAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManMeleeAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManWindMill>());
    }
}
