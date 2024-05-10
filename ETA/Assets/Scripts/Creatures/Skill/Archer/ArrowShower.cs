using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShower : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(10);
        Damage = 30;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(7, 7, 7);
        //RangeType = Define.RangeType.Square;
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ArrowShower.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);

        yield return new WaitForSeconds(0.2f);
        //Managers.Sound.Play("Skill/ArrowShot");

        //yield return new WaitForSeconds(0.7f);

        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage);
        //hitbox.transform.position = _skillSystem.TargetPosition;
        //hitbox.transform.localScale = skillRange;
        //yield return new WaitForSeconds(0.1f);
        //Managers.Resource.Destroy(hitbox.gameObject);
        //Managers.Sound.Play("Skill/ArrowShowerHit");
        ////ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowShower").GetComponent<ParticleSystem>();
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.ArrowShower, 1.0f, gameObject.transform);
        //ps.transform.position = hitbox.transform.position;
        //ps.Play();

        Managers.Coroutine.Run(ArrowShowerCoroutine());

        yield return new WaitForSeconds(0.8f);
        //Managers.Resource.Destroy(ps.gameObject);

        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    IEnumerator ArrowShowerCoroutine()
    {
        Managers.Sound.Play("Skill/ArrowShot");

        yield return new WaitForSeconds(0.7f);

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Sound.Play("Skill/ArrowShowerHit");
        //ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowShower").GetComponent<ParticleSystem>();
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ArrowShower, 1.0f, gameObject.transform);
        ps.transform.position = hitbox.transform.position;
    }
}
