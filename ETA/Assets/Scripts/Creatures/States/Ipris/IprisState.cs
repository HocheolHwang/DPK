using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EIprisPattern
{
    BUFF,
    PatternOneEnable,
    PatternOneAttack,
    PatternOneStrongAttack,
    CounterEnable,
    CounterAttack,
    MAX_LEN,
}

public class IprisState : State
{
    protected IprisController _controller;
    protected IprisAnimationData _animData;

    public IprisState(IprisController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }
}
