using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KnightG 상태
public class KnightGState : State
{
    protected KnightGController _controller;
    protected KnightGAnimationData _animData;


    public KnightGState(KnightGController controller) : base(controller)
    {
        _controller = controller;
        _animData = controller.KnightGAnimData;
    }

    // ----------------------------- Common Functions -------------------------------------
    public bool IsStayForSeconds()
    {
        if (ExecuteTime < 0.5f) return false;
        return true;
    }
}
