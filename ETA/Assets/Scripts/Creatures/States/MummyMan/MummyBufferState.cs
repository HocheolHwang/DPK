using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum EBufferPattern
{
    RangedAutoAttack = 0,
    MAX_LEN
}

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
