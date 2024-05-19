using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpiritInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.ForestSpirit;
        PatternList.Add(gameObject.GetOrAddComponent<ForestSpilitAutoAttack>());
    }
}
