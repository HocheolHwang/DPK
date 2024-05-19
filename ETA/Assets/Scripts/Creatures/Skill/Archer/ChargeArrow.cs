using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeArrow : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(15);
        Damage = 50;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 12);
        //CollavoSkillName = "ChargeArrow";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("HOLD", 0.1f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ChargeArrowCharge, 1.0f, gameObject.transform);
        yield return new WaitForSeconds(1.0f);
        
        Managers.Coroutine.Run(ChargeArrowCoroutine());

        yield return new WaitForSeconds(1.0f);

        ChangeToPlayerMoveState();
    }

    IEnumerator ChargeArrowCoroutine()
    {
        _animator.CrossFade("HOLDATTACK", 0.1f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/WindBlast");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _controller.Stat.AttackDamage + 30, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = _skillSystem.TargetPosition + transform.forward * 2.0f;
        hitbox.transform.localScale = skillRange;

        ParticleSystem ps01 = Managers.Effect.Play(Define.Effect.ChargeArrow, 1.0f, gameObject.transform);
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    //public override IEnumerator StartCollavoSkillCast()
    //{
    //    _animator.CrossFade("SKILL6", 0.1f);

    //    yield return new WaitForSeconds(0.1f);

    //    ChangeToPlayerMoveState();
    //}
}
