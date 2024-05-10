using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PorinInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Porin;
        PatternList.Add(gameObject.GetOrAddComponent<PorinAutoAttack>());
    }
}
