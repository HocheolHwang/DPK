using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyInfo : PatternInfo
{

    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Butterfly;
        PatternList.Add(gameObject.GetOrAddComponent<ButterflyAutoAttack>());
    }
}
