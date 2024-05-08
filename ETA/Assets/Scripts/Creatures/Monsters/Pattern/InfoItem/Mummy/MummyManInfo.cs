using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pattern DMG
/// </summary>
public class MummyManInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        PatternList.Add(gameObject.GetOrAddComponent<MummyManRangedAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyManMeleeAutoAttack>());
    }
}
