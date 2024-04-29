using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<KnightGFirstAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<KnightGSecondAutoAttack>());
        // counter
        PatternList.Add(gameObject.GetOrAddComponent<KnightGCounterEnablePattern>());
        PatternList.Add(gameObject.GetOrAddComponent<KnightGCounterAttackPattern>());
    }
}
