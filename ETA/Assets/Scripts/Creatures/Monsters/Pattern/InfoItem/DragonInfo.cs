using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pattern INFO( ATK: 공격력, P_ATK: 패턴 공격력 )
/// AttackDown: ATK + knock back
/// AttackSwing: ATK
/// AttackTail: ATK + knock back
/// </summary>
public class DragonInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        // GROUND
        PatternList.Add(gameObject.GetOrAddComponent<DragonAttackDown>());
        PatternList.Add(gameObject.GetOrAddComponent<DragonAttackSwing>());
        PatternList.Add(gameObject.GetOrAddComponent<DragonAttackTail>());

        PatternList.Add(gameObject.GetOrAddComponent<DragonFearEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<DragonFear>());
        PatternList.Add(gameObject.GetOrAddComponent<DragonFearStrong>());

        PatternList.Add(gameObject.GetOrAddComponent<DragonBreathEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<DragonBreath>());

        // SKY
    }
}
