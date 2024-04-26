using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class MummyWarriorState : State
{
    protected static float _windMillTime;

    protected MummyWarriorController _controller;
    protected MummyWarriorAnimationData _animData;

    public MummyWarriorState(MummyWarriorController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }
}
