using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EDragonPattern
{
    ATTACK_DOWN         = 0,
    ATTACK_SWING,
    ATTACK_TAIL,
    FEAR_ENABLE,
    FEAR,
    FEAR_STRONG,
    BREATH_ENABLE,
    BREATH,
    BREATH_GROGGY,
    CRY_TO_DOWN,
    SKY_DOWN_ATTACK,
    CRY_TO_FIRE,
    FLY_FIREBALL,
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

    // ---------------------- SET DEF --------------------------
    public void IncreaseDEF()
    {
        _controller.IncreaseDefense(_controller.AmountDEF);
    }

    public void DecreaseDEF()
    {
        _controller.DecreaseDefense(_controller.AmountDEF);
    }
}
