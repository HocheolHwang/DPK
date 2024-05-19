using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBlast : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 30;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(5, 5, 12);
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("ATTACK1", 0.1f);
        yield return new WaitForSeconds(0.1f);

        Managers.Coroutine.Run(WindBlastCoroutine());

        yield return new WaitForSeconds(0.1f);
        ChangeToPlayerMoveState();
    }

    IEnumerator WindBlastCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        ParticleSystem ps01 = Managers.Effect.Play(Define.Effect.ArrowStab, 1.0f, gameObject.transform);
        ParticleSystem ps02 = Managers.Effect.Play(Define.Effect.WindBlast, 1.0f, gameObject.transform);
        Managers.Sound.Play("Skill/WindBlast");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage, -1, true);
        hitbox.SetUp(transform, _controller.Stat.AttackDamage + 10, -1, true);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = _skillSystem.TargetPosition + transform.forward;
        hitbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
