using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EBufferPattern
{
    RangedAutoAttack = 0,
    CounterEnable,
    Buff,
    MAX_LEN
}

public class MummyBufferState : State
{
    protected static float _buffTime = _threadHoldBuff - 2.0f;
    protected const float _threadHoldBuff = 30f;           // 30초

    protected MummyBufferController _controller;
    protected MummyBufferAnimationData _animData;

    public MummyBufferState(MummyBufferController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }
}
