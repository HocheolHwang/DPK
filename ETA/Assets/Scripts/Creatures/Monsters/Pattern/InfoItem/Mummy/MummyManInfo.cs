using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// 1. MeleeAttack: ATK, ATK, [ ATK + P_ATK(20) ] * 2
/// 2. RangedAttack: ATK + P_ATK(5)
/// 3. WindMill: ATK + P_ATK(20)
/// 4. Jump: ATK + P_ATK(40)
/// 5. Shouting: ATK + P_ATK(20)
/// 6. Rush: ATK + P_ATK(50, 150)           - counter 이후 공격
/// </summary>
public class MummyManInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        PatternList.Add(gameObject.GetOrAddComponent<MummyManRangedAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManMeleeAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManWindMill>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManJump>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManShouting>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManRush>());
    }
}
