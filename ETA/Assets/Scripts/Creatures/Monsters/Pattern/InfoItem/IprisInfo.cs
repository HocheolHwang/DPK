using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Pattern DMG( ATK: 공격력, P_ATK: 패턴 공격력 )
/// </summary>
public class IprisInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        //PatternList.Add(gameObject.GetOrAddComponent<AutoAttack>());
    }
}
