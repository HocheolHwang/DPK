using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class MonsterState : State
{
    protected MonsterController _controller;
    protected MonsterAnimationData _animData;
    protected MonsterStat _monsterStat;

    private float _seconds = 0.5f;

    public MonsterState(MonsterController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.animData;
        _monsterStat = controller.monsterStat;
    }

    // _seconds 만큼 기다림
    public bool IsStayForSeconds()
    {
        // Debug.Log($"{StartTime} | {ExecuteTime}");
        if (ExecuteTime < _seconds) return false;
        return true;
    }
}
