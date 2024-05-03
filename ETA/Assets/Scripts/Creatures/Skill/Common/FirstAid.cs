using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAid : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(30);
        SkillType = Define.SkillType.Immediately;
        base.Init();
    }

    public override IEnumerator StartSkillCast()
    {
        Managers.Sound.Play("Skill/Heal");
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.HealEffect, gameObject.transform);
        _controller.Stat.Hp += 100;
        if (_controller.Stat.Hp > _controller.Stat.MaxHp)
            _controller.Stat.Hp = _controller.Stat.MaxHp;
        Debug.Log(_controller.Stat.Hp);

        yield return new WaitForSeconds(1.0f);
        Managers.Effect.Stop(ps);

        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
