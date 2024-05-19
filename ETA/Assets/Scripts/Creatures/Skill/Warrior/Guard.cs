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
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Guard.png");
    }

    public override IEnumerator StartSkillCast()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        _animator.CrossFade("GUARD", 0.05f);
        yield return new WaitForSeconds(0.05f);
        Managers.Sound.Play("Skill/GuardSkill");
        Managers.Coroutine.Run(GetShieldCoroutine());

        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    IEnumerator GetShieldCoroutine()
    {
        _controller.GetShield(100 + Damage);
        yield return new WaitForSeconds(1.0f);

        _controller.RemoveShield(100 + Damage);
    }
}
