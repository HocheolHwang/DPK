using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlash : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(20);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(3, 3, 3);
        CollavoSkillRange = new Vector3(15, 15, 15);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/WindSlash.png");
        CollavoSkillName = "Cyclone";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("COLLAVO", 0.1f);
        yield return new WaitForSeconds(0.1f);
        //Managers.Sound.Play("Skill/RSkill");
        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage, -1, true);
        //hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        //hitbox.transform.localScale = skillRange;

        ////ParticleSystem ps = Managers.Resource.Instantiate("Effect/WindSlash").GetComponent<ParticleSystem>();
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindSlash, 1.0f, gameObject.transform);
        //ps.transform.position = gameObject.transform.position + transform.up;

        ////ps.Play();
        //yield return new WaitForSeconds(0.1f);
        //Managers.Resource.Destroy(hitbox.gameObject);

        Managers.Coroutine.Run(WindSlashCoroutine());

        yield return new WaitForSeconds(1.0f);
        //Managers.Resource.Destroy(ps.gameObject);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("COLLAVO", 0.1f);
        yield return new WaitForSeconds(0.5f);

        //ParticleSystem ps0 = Managers.Resource.Instantiate("Effect/CycloneUIEffect").GetComponent<ParticleSystem>();
        //ps0.Play();

        //ParticleSystem ps = Managers.Resource.Instantiate("Effect/WindSlash").GetComponent<ParticleSystem>();
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindSlash, 1.0f, gameObject.transform);
        //ps.transform.position = gameObject.transform.position + transform.up;
        ////ps.Play();

        ////ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/CollavoCyclone").GetComponent<ParticleSystem>();
        //ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.CollavoCyclone, 2.0f, gameObject.transform);
        //ps1.transform.position = transform.position;
        ////ps1.Play();

        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage);
        //hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
        //hitbox.transform.localScale = CollavoSkillRange;

        ////ParticleSystem ps2 = Managers.Resource.Instantiate("Effect/CollavoCycloneShot").GetComponent<ParticleSystem>();
        //ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.CollavoCycloneShot, 1.0f, gameObject.transform);
        //ps2.transform.position = transform.position;

        //for (int i = 0; i < 10; i++)
        //{
        //    yield return new WaitForSeconds(0.20f);
        //    //ps2.Play();

        //    Managers.Resource.Destroy(hitbox.gameObject);
        //    hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //    hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
        //    hitbox.transform.localScale = CollavoSkillRange;
        //    Managers.Sound.Play("Skill/TargetSkill");
        //}

        //yield return new WaitForSeconds(0.1f);
        //Managers.Resource.Destroy(hitbox.gameObject);

        ParticleSystem psUI = Managers.Effect.Play(Define.Effect.WarriorArcherUIEffect, 2.0f, gameObject.transform);

        Managers.Coroutine.Run(WindSlashCollavoCoroutine());

        yield return new WaitForSeconds(1.0f);
        //Managers.Resource.Destroy(ps.gameObject);
        //Managers.Resource.Destroy(ps0.gameObject);
        //Managers.Resource.Destroy(ps1.gameObject);
        //Managers.Resource.Destroy(ps2.gameObject);

        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    IEnumerator WindSlashCoroutine()
    {
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, false);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;

        //ParticleSystem ps = Managers.Resource.Instantiate("Effect/WindSlash").GetComponent<ParticleSystem>();
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindSlash, 1.0f, gameObject.transform);
        ps.transform.position = gameObject.transform.position + transform.up;

        //ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    IEnumerator WindSlashCollavoCoroutine()
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.WindSlash, 1.0f, gameObject.transform);
        ps.transform.position = gameObject.transform.position + transform.up;
        //ps.Play();

        //ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/CollavoCyclone").GetComponent<ParticleSystem>();
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.CollavoCyclone, 2.0f, gameObject.transform);
        ps1.transform.position = transform.position;
        //ps1.Play();

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
        hitbox.transform.localScale = CollavoSkillRange;

        //ParticleSystem ps2 = Managers.Resource.Instantiate("Effect/CollavoCycloneShot").GetComponent<ParticleSystem>();
        ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.CollavoCycloneShot, 1.0f, gameObject.transform);
        ps2.transform.position = transform.position;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.20f);
            //ps2.Play();

            Managers.Resource.Destroy(hitbox.gameObject);
            hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
            hitbox.transform.localScale = CollavoSkillRange;
            Managers.Sound.Play("Skill/TargetSkill");
        }

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }
}
