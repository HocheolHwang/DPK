using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EDragonPattern
{
    MAX_LEN,
}

public class DragonState : State
{
    protected DragonController _controller;
    protected DragonAnimationData _animData;

    public DragonState(DragonController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }
}
