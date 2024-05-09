using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyWarriorInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        PatternList.Add(gameObject.GetOrAddComponent<MummyWarriorMeleeAutoAttack>());
        PatternList.Add(gameObject.GetOrAddComponent<MummyWarriorWindMill>());
    }
}
