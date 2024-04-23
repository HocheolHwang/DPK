using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KnightG 상태
public class KnightGState : MonsterState
{

    public KnightGState(KnightGController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.AnimData;
    }

}
