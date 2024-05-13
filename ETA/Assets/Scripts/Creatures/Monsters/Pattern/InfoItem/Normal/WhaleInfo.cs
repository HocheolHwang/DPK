using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<WhaleFirstAutoAttack>());
        // counter
        PatternList.Add(gameObject.GetOrAddComponent<WhaleCounterEnable>());
        PatternList.Add(gameObject.GetOrAddComponent<WhaleCounterAttackPattern>());

    }
}
