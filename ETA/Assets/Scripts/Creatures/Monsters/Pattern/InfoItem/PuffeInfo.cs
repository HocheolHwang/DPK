using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuffeInfo : PatternInfo
{    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Puffe;
        PatternList.Add(gameObject.GetOrAddComponent<PuffeAutoAttack>());
    }
}
