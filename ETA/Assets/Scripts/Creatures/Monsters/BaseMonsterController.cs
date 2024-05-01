using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

// 모든 몬스터가 공통으로 가지는 데이터와 기능을 관리
public class BaseMonsterController : BaseController
{
    [Header("is hit counter? - 디버깅")]
    [SerializeField] private bool _isHitCounter;
    protected PatternInfo _patternInfo;

    public PatternInfo PatternInfo { get => _patternInfo; }
    public bool IsHitCounter { get => _isHitCounter; private set => _isHitCounter = value; }

    protected override void Awake()
    {
        base.Awake();
        _patternInfo = GetComponent<PatternInfo>();
    }

    protected virtual void Start()
    {
        StartCoroutine(InitCounter());      // Boss Controller만 상속
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Init()
    {
    }

    // ---------------------------------- Hit Counter Event ------------------------------------------
    // 카운터, 히든 기믹 파훼 판단
    public override void CounterEvent()
    {
        base.CounterEvent();
        _isHitCounter = true;
    }

    IEnumerator InitCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.15f);
            if (_isHitCounter)
            {
                
                _isHitCounter = false;
            }
        }
        // counter 맞고 0.15초 뒤에 맞았다는 상태를 초기화

    }
}
