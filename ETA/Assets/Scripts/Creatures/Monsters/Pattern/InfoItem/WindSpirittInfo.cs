using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpirittInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.WindSpirit;
        PatternList.Add(gameObject.GetOrAddComponent<WindSpiritAutoAttack>());
    }
}
