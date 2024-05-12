using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantaInfo : PatternInfo
{
    protected override void Init()
    {
        base.Init();
        GetComponent<MonsterController>().UnitType = Define.UnitType.Planta;
        PatternList.Add(gameObject.GetOrAddComponent<PlantaAutoAttack>());
    }
}
