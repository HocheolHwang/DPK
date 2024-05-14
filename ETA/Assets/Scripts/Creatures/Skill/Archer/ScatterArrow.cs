using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterArrow : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 30;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(5, 5, 5);
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL4", 0.1f);
        yield return new WaitForSeconds(0.1f);

        Managers.Coroutine.Run(ScatterArrowCoroutine());

        yield return new WaitForSeconds(0.1f);
        ChangeToPlayerMoveState();
    }

    IEnumerator ScatterArrowCoroutine()
    {
        Managers.Sound.Play("Skill/ArrowStab");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = _skillSystem.TargetPosition + transform.forward;
        hitbox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ScatterArrow, 1.0f, gameObject.transform);

        yield return new WaitForSeconds(0.1f);

    }
}
