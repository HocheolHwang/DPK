using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSkill : TmpSkill
{
    protected override void Init()
    {
        SetCoolDownTime(2);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(7, 7, 7);
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("GUARD", 0.05f);
        yield return new WaitForSeconds(0.1f);
        _controller.GetShield(100);
        yield return new WaitForSeconds(1.0f);

        _controller.RemoveShield(100);

        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
