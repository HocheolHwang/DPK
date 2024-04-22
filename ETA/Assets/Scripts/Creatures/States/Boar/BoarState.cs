using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class BoarState : State
{
    protected BoarController _controller;
    protected BoarAnimationData _animData;

    public BoarState(BoarController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.animData;
    }
}
