using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EIprisPattern
{
    BUFF                      = 0,
    PatternOneEnable,
    PatternOneAttack,
    PatternOneStrongAttack,
    CounterEnable,
    CounterAttack,
    PatternTwo,
    PatternTwoWindMill,
    ATTACK,
    MAX_LEN,
}

public class IprisState : State
{
    protected const float PT_time = 3.0f;       // Pattern Two 이동시간

    protected IprisController _controller;
    protected IprisAnimationData _animData;

    public IprisState(IprisController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }

    // -------------------------- PATTERN_TWO FUNCTIONS -----------------------------------
    public float CalcSpeedFromDestTime(Vector3 Destination)
    {
        float remainDist = Vector3.Distance(Destination, _controller.transform.position);

        return remainDist / PT_time;
    }
}
