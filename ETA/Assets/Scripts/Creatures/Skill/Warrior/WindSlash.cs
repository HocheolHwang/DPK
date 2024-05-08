using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSlash : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(3, 3, 3);
        CollavoSkillRange = new Vector3(15, 15, 15);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/WindBall.png");
        CollavoSkillName = "Cyclone";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("COLLAVO", 0.1f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Resource.Instantiate("Effect/WindSlash").GetComponent<ParticleSystem>();
        ps.transform.position = gameObject.transform.position + transform.up;

        ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.0f);
        Managers.Resource.Destroy(ps.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("COLLAVO", 0.1f);
        yield return new WaitForSeconds(0.5f);

        ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/CollavoCyclone").GetComponent<ParticleSystem>();
        ps1.transform.position = transform.position;
        ps1.Play();

        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 4;
        hitbox.transform.localScale = CollavoSkillRange;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.0f);
        Managers.Resource.Destroy(ps1.gameObject);

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
