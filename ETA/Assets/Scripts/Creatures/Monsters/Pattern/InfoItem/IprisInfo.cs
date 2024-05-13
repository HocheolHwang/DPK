using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// BUFF: 영구적인 증가 버프 - ATK+2, DEF+1, Shield( hp 15% ), 쿨타임 15초
/// COUNTER_ATTACK: ATK + P_ATK(100)
/// </summary>
public class IprisInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<IprisBuff>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisCounterEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisCounterAttack>());
    }
}
