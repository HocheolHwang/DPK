using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(2);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Guard.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("GUARD", 0.05f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/GuardSkill");
        _controller.GetShield(100);
        yield return new WaitForSeconds(1.0f);

        _controller.RemoveShield(100);

        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
