using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각 상태 클래스의 중복 코드를 막기 위함
public class BoarState : State
{
    protected BoarController _controller;
    protected Detector _detector;
    protected BoarAnimationData _animData;

    public BoarState(BoarController controller) : base(controller)
    {
        _controller = controller;
        _detector = controller.detector;
        _animData = controller.animData;
    }
}
