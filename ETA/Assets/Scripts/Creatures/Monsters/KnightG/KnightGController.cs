using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGController : MonsterController
{
    // KnightG만 가지는 상태

    protected override void Init()
    {
        base.Init();

        _animData = GetComponent<KnightGAnimationData>();
        _animData.StringAnimToHash();
    }

    // ---------------------------------- IDamage ------------------------------------------
    public override void DestroyEvent()
    {
    }
}
