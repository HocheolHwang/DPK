using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EIprisPattern
{
    MAX_LEN,
}

public class IprisState : State
{
    protected IprisController _controller;
    protected IprisAnimationData _animData;
    protected Transform _target;

    public IprisState(IprisController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
        _target = controller.Detector.Target;
    }
}
