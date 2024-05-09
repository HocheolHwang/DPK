using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningShot : Skill
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
        CollavoSkillName = "BlackHoll";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);
        yield return new WaitForSeconds(0.6f);
        Managers.Sound.Play("Skill/ArrowStab");
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.LightningShot, 1.0f, gameObject.transform);
        ps.transform.position = gameObject.transform.position + transform.up;
        ps.Play();

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2.0f;
        hitbox.transform.rotation = gameObject.transform.rotation;
        hitbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.0f);
        //Managers.Resource.Destroy(ps.gameObject);
        //Managers.Effect.Stop(ps);

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("SKILL6", 0.1f);
        yield return new WaitForSeconds(0.5f);

        ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/CollavoWindBall").GetComponent<ParticleSystem>();
        ps1.transform.position = transform.position;
        ps1.Play();
        yield return new WaitForSeconds(1.0f);

        Managers.Effect.Stop(ps1);
        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
