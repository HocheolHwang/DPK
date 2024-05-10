using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFishInfo : PatternInfo
{    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Starfish;
        PatternList.Add(gameObject.GetOrAddComponent<StarFishAutoAttack>());
    }
}
