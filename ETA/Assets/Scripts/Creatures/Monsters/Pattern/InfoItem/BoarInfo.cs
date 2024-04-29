using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarInfo : PatternInfo
{

    protected override void Init()
    {
        base.Init();

        PatternList.Add(gameObject.GetOrAddComponent<BoarAutoAttack>());
    }
}
