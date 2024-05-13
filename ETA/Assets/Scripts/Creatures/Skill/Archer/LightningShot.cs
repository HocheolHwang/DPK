using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LightningShot : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(20);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(5, 5, 5);
        CollavoSkillRange = new Vector3(15, 15, 15);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/WindBall.png");
        CollavoSkillName = "BlackHole";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);
        yield return new WaitForSeconds(0.6f);
        //Managers.Sound.Play("Skill/ArrowShot");
        //ParticleSystem ps = Managers.Effect.Play(Define.Effect.LightningShot, 1.0f, gameObject.transform);
        //ps.transform.position = gameObject.transform.position + transform.up;
        ////ps.Play();
        //Managers.Sound.Play("Skill/LightningShot");
        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage, -1, false);
        ////hitbox.transform.position = gameObject.transform.position + transform.forward;
        //hitbox.transform.position = gameObject.transform.position + transform.forward * 2.0f;
        //hitbox.transform.rotation = gameObject.transform.rotation;
        //hitbox.transform.localScale = skillRange;

        Managers.Coroutine.Run(LightningShotCoroutine());

        //yield return new WaitForSeconds(0.1f);
        //Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(0.3f);
        //Managers.Resource.Destroy(ps.gameObject);
        //Managers.Effect.Stop(ps);

        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);
        yield return new WaitForSeconds(0.6f);

        //ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/CollavoWindBall").GetComponent<ParticleSystem>();
        //ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.CollavoBlackHole, 2.0f, gameObject.transform);
        //ps1.transform.position = transform.position;
        ////ps1.Play();
        //Managers.Sound.Play("Skill/CollavoBlackhole");

        //HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //hitbox.SetUp(transform, Damage);
        //hitbox.transform.position = gameObject.transform.position + transform.forward * 8;
        //hitbox.transform.localScale = CollavoSkillRange;

        //for (int i = 0; i < 10; i++)
        //{
        //    yield return new WaitForSeconds(0.20f);
        //    //ps2.Play();

        //    Managers.Resource.Destroy(hitbox.gameObject);
        //    hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        //    hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
        //    hitbox.transform.localScale = CollavoSkillRange;
        //    Managers.Sound.Play("Skill/CollavoBlackholeHit");
        //}
        //Managers.Effect.Stop(ps1);
        //ParticleSystem psUI = Managers.Effect.Play(Define.Effect.ArcherMageUIEffect, 1.0f, gameObject.transform);

        ParticleSystem psUI = Managers.Effect.Play(Define.Effect.ArcherMageUIEffect, 2.0f, gameObject.transform);

        Managers.Coroutine.Run(LightningShotCollavoCoroutine());

        yield return new WaitForSeconds(0.1f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }

    IEnumerator LightningShotCoroutine()
    {
        Managers.Sound.Play("Skill/ArrowShot");
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.LightningShot, 1.0f, gameObject.transform);
        ps.transform.position = gameObject.transform.position + transform.up;
        //ps.Play();
        Managers.Sound.Play("Skill/LightningShot");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2.0f;
        hitbox.transform.rotation = gameObject.transform.rotation;
        hitbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
    }

    IEnumerator LightningShotCollavoCoroutine()
    {
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.CollavoBlackHole, 2.0f, gameObject.transform);
        ps1.transform.position = transform.position;
        //ps1.Play();
        Managers.Sound.Play("Skill/CollavoBlackhole");

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 8;
        hitbox.transform.localScale = CollavoSkillRange;

        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.20f);
            //ps2.Play();

            Managers.Resource.Destroy(hitbox.gameObject);
            hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
            hitbox.transform.localScale = CollavoSkillRange;
            Managers.Sound.Play("Skill/CollavoBlackholeHit");
        }
    }
}
