using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStab : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 30;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 8);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ArrowStab.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("ATTACK1", 0.1f);
        yield return new WaitForSeconds(0.1f);
        //Managers.Sound.Play("Skill/ArrowStab");
        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage, -1, false);
        ////hitbox.transform.position = gameObject.transform.position + transform.forward;
        //hitbox.transform.position = _skillSystem.TargetPosition - transform.forward ;
        //hitbox.transform.localScale = skillRange;

        ////ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowStab").GetComponent<ParticleSystem>();
        //yield return new WaitForSeconds(0.2f);
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.ArrowStab, 1.0f, gameObject.transform);
        ////ps.transform.position = gameObject.transform.position + transform.forward;

        ////ps.Play();
        //yield return new WaitForSeconds(0.1f);
        //Managers.Resource.Destroy(hitbox.gameObject);

        Managers.Coroutine.Run(ArrowStabCoroutine());
        yield return new WaitForSeconds(0.3f);
        //Managers.Resource.Destroy(ps.gameObject);
        //Managers.Effect.Stop(ps);

        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    IEnumerator ArrowStabCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/ArrowStab");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, _controller.Stat.AttackDamage, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = _skillSystem.TargetPosition + transform.forward;
        hitbox.transform.localScale = skillRange;

        //ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowStab").GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(0.2f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ArrowStab, 1.0f, gameObject.transform);
        //ps.transform.position = gameObject.transform.position + transform.forward;

        //ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
