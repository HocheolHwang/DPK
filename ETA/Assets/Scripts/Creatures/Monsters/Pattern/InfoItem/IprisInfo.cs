using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// BUFF: 영구적인 증가 버프 - ATK+2, DEF+1, Shield( hp 15% ), 쿨타임 15초
/// PATTERN_ONE_ENABLE: 빨간색 COUNTER EFFECT
/// PATTERN_ONE_STRONG_ATTACK: [ ATK + P_ATK(50) ] * 1번
/// PATTERN_ONE_STRONG_ATTACK: [ ATK + P_ATK(50) ] * 3번
/// COUNTER_ATTACK: ATK + P_ATK(100) | P_ATK(100)
/// PATTERN_TWO: ATK + P_ATK(50)
/// PATTERN_TWO_WindMill: [ ATK + P_ATK(50) ] * 2번
/// </summary>
public class IprisInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<IprisBuff>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisPatternOneEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisPatternOneAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisPatternOneStrongAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisCounterEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisCounterAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisPatternTwo>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisPatternTwoWindMill>());
        PatternList.Add(gameObject.GetOrAddComponent<IprisAttack>());
    }
}
