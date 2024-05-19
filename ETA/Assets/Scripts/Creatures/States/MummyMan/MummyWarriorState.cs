using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EMummyWarriorPattern
{
    MeleeAutoAttack = 0,
    WindMill,
    MAX_LEN
}

public class MummyWarriorState : State
{
    protected MummyWarriorController _controller;
    protected MummyWarriorAnimationData _animData;

    public MummyWarriorState(MummyWarriorController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }
}
