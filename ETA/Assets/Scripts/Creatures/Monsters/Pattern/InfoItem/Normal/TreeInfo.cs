using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeInfo : PatternInfo
{

    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Tree;
        PatternList.Add(gameObject.GetOrAddComponent<TreeAutoAttack>());
    }
}
