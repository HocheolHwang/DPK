using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormStrike : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(13);
        Damage = 100;
        base.Init();
        SkillType = Define.SkillType.Target;
        skillRange = new Vector3(1, 1, 1);
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("HOLD", 0.1f);
        Managers.Coroutine.Run(StormStrikeChargeCoroutine());
        yield return new WaitForSeconds(2.0f);

        _animator.CrossFade("HOLDATTACK", 0.1f);
        yield return new WaitForSeconds(0.1f);
        
        Managers.Coroutine.Run(StormStrikeCoroutine());
        yield return new WaitForSeconds(0.8f);
        

        yield return new WaitForSeconds(0.1f);
        ChangeToPlayerMoveState();
    }

    IEnumerator StormStrikeCoroutine()
    {
        // 발사 이펙트
        ParticleSystem ps01 = Managers.Effect.Play(Define.Effect.StormStrikeFire01, 1.0f, gameObject.transform);
        yield return new WaitForSeconds(0.1f);
        ParticleSystem ps02 = Managers.Effect.Play(Define.Effect.StormStrikeFire02, 1.0f, gameObject.transform);
        Managers.Sound.Play("Skill/ArrowStab");

        // 타격 이펙트
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/StormStrikeHit");
        ParticleSystem ps03 = Managers.Effect.Play(Define.Effect.StormStrikeHit, 1.0f, gameObject.transform);
        ps03.transform.position = _skillSystem.TargetPosition;

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _controller.Stat.AttackDamage + 80, -1, true);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    IEnumerator StormStrikeChargeCoroutine()
    {
        // 기모으기 이펙트
        Managers.Sound.Play("Skill/StormStrikeCharge");

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.StormStrikeCharge, 2.2f, gameObject.transform);

        yield return new WaitForSeconds(0.1f);
    }
}
