using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class MonsterState : State
{
    protected MonsterController _controller;
    protected MonsterAnimationData _animData;

    public MonsterState(MonsterController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }


    // ----------------------------- Common Functions -------------------------------------
    public bool IsStayForSeconds()
    {
        // Debug.Log($"{StartTime} | {ExecuteTime}");
        if (ExecuteTime < 0.5f) return false;
        return true;
    }
}
