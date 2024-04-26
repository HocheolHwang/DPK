using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class MummyBufferState : State
{
    protected static float _buffTime;

    protected MummyBufferController _controller;
    protected MummyBufferAnimationData _animData;

    public MummyBufferState(MummyBufferController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }
}
