using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 몬스터가 공통으로 가지는 데이터와 기능을 관리
public class BaseMonsterController : BaseController
{
    protected PatternInfo _patternInfo;

    public PatternInfo PatternInfo { get => _patternInfo; }

    protected override void Awake()
    {
        base.Awake();
        _patternInfo = GetComponent<PatternInfo>();
    }

    protected virtual void Start()
    {
    }

    protected override void Init()
    {
    }
}
