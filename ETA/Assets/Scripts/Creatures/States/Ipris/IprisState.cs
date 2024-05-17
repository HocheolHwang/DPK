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
    protected IprisController _controller;
    protected IprisAnimationData _animData;

    public IprisState(IprisController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }

    // -------------------------- PATTERN_TWO FUNCTIONS -----------------------------------
    public float CalcTimeToDest(Vector3 Destination)
    {
        float moveSpeed = _agent.speed;
        if (moveSpeed <= 0.1f)
        {
            Debug.Log($"{_controller.gameObject.name}의 속도({moveSpeed})가 0.1f보다 낮습니다.");
            return -1;
        }
        else if (moveSpeed > 8.0f)
        {
            moveSpeed = 8.0f;
        }

        float remainDist = Vector3.Distance(Destination, _controller.transform.position);
        if (remainDist < 2.0f)
        {
            remainDist = 2.0f;
        }

        float timeToDest = remainDist / moveSpeed;
        if (timeToDest < 3.0f)
        {
            timeToDest = 3.0f;
        }
        return timeToDest;
    }
}
