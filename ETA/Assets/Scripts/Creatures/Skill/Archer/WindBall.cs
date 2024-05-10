using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class WindBall : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(5, 5, 5);
        CollavoSkillRange = new Vector3(10, 10, 10);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/WindBall.png");
        CollavoSkillName = "Cyclone";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("COLLAVO", 0.1f);
        yield return new WaitForSeconds(0.1f);
        //Managers.Sound.Play("Skill/ArrowStab");
        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage, -1, false);
        ////hitbox.transform.position = gameObject.transform.position + transform.forward;
        //hitbox.transform.position = _skillSystem.TargetPosition;
        //hitbox.transform.localScale = skillRange;

        ////ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowStab").GetComponent<ParticleSystem>();
        //yield return new WaitForSeconds(0.2f);
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindBall, 1.0f, gameObject.transform);
        //ps.transform.position = gameObject.transform.position + transform.up;

        ////ps.Play();
        //yield return new WaitForSeconds(0.1f);
        //Managers.Resource.Destroy(hitbox.gameObject);

        Managers.Coroutine.Run(WindBallCoroutine());

        yield return new WaitForSeconds(1.0f);
        //Managers.Resource.Destroy(ps.gameObject);
        //Managers.Effect.Stop(ps);

        yield return new WaitForSeconds(0.1f);
        //if (_controller.photonView.IsMine) _controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("SKILL6", 0.1f);
        yield return new WaitForSeconds(0.5f);

        //ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/CollavoWindBall").GetComponent<ParticleSystem>();
        //ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.CollavoWindBall, 1.0f, gameObject.transform);

        //ps1.transform.position = transform.position;
        //ps1.Play();

        Managers.Coroutine.Run(WindBallCollavoCoroutine());

        //yield return new WaitForSeconds(1.0f);

        //Managers.Effect.Stop(ps1);
        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    IEnumerator WindBallCoroutine()
    {
        Managers.Sound.Play("Skill/ArrowStab");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = _skillSystem.TargetPosition;
        hitbox.transform.localScale = skillRange;

        //ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowStab").GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(0.2f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindBall, 1.0f, gameObject.transform);
        ps.transform.position = gameObject.transform.position + transform.up;

        //ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    IEnumerator WindBallCollavoCoroutine()
    {
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.CollavoWindBall, 1.5f, gameObject.transform);
        ps1.transform.position = transform.position;
        yield return new WaitForSeconds(1.0f);
    }
}
