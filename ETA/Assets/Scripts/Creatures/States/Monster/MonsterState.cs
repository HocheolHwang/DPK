using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class MonsterState : State
{
    protected float _animTime;                          // 각 상태의 animation 수행 시간
    protected float _threadHold;                        // 각 상태의 animation 끝나는 시간

    protected MonsterController _controller;
    protected MonsterAnimationData _animData;

    public MonsterState(MonsterController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }


    // ----------------------------- Common Functions -------------------------------------
    public bool IsStayForSeconds(float seconds = 0.5f)
    {
        if (ExecuteTime < seconds) return false;
        return true;
    }

    public void InitTime(float animLength)
    {
        _animTime = 0;
        _threadHold = animLength;
    }

    public void LookAtEnemy()
    {
        if (_detector.Target == null) return;
        Vector3 dir = _detector.Target.position;
        dir.y = _controller.transform.position.y;
        _controller.transform.LookAt(dir);
    }
}
