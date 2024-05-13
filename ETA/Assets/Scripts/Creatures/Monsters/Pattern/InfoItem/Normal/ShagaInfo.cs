using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShagaInfo : PatternInfo
{

    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Shellfish;
        PatternList.Add(gameObject.GetOrAddComponent<ShagaAutoAttack>());
    }
}
