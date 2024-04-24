using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGController : MonsterController
{
    // 보스몬스터 사망 액션 선언
    public static event Action OnBossDestroyed;

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
        // 보스몬스터 죽었을때 이벤트 발생
        OnBossDestroyed?.Invoke();
    }
}
