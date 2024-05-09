using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KraKeInfo : PatternInfo
{    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Krake;
        PatternList.Add(gameObject.GetOrAddComponent<KrakeAutoAttack>());
    }
}
