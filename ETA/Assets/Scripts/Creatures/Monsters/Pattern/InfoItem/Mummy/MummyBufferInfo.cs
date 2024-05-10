using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// 1. RangedAttack: ATK
/// 2. Buff: 모두에게 버프 수치( HP 10% 회복, ATK += 10, DEF += 5, TIME: 30초 쿨타임과 유지시간 )
/// </summary>
public class MummyBufferInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        PatternList.Add(gameObject.GetOrAddComponent<MummyBufferRangedAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyBufferBuff>());
    }
}
