using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class StormArrow : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 30;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(7, 7, 7);
    }
    public override IEnumerator StartSkillCast()
    {
        Managers.Coroutine.Run(ArrowShowerCoroutine());

        yield return new WaitForSeconds(0.3f);

        yield return new WaitForSeconds(0.1f);
        ChangeToPlayerMoveState();
    }

    IEnumerator ArrowShowerCoroutine()
    {
        Managers.Sound.Play("Skill/ArrowShot");


        for (int i = 0; i < 4; i++)
        {
            _animator.CrossFade("SKILL2", 0.1f);

            yield return new WaitForSeconds(0.50f);

            ParticleSystem ps02 = Managers.Effect.Play(Define.Effect.ArrowShower, 1.0f, gameObject.transform);

            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = _skillSystem.TargetPosition;
            hitbox.transform.localScale = skillRange;
            Managers.Resource.Destroy(hitbox.gameObject);
            Managers.Sound.Play("Skill/ArrowShowerHit");

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.1f);
    }
}
