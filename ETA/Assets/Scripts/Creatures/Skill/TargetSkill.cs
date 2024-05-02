using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 50;
        base.Init();
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);
        //SwordVolleyBlue
        yield return new WaitForSeconds(0.1f);
        ParticleSystem ps1 = Managers.Resource.Instantiate("Effect/SwordVolleyBlue").GetComponent<ParticleSystem>();
        ps1.transform.position = _skillSystem.TargetPosition + gameObject.transform.up;
        ps1.Play();
        Managers.Sound.Play("Skill/TargetSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        yield return new WaitForSeconds(0.2f);
        Managers.Resource.Destroy(hitbox.gameObject);
        Managers.Sound.Play("Skill/TargetSkill");
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/TargetSkill");
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/TargetSkill");
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/TargetSkill");
        yield return new WaitForSeconds(0.2f);
        Managers.Sound.Play("Skill/TargetSkill");
        yield return new WaitForSeconds(0.3f);
        _controller.ChangeState(_controller.MOVE_STATE);
        yield return new WaitForSeconds(0.4f);
        Managers.Resource.Destroy(ps1.gameObject);
        
    }
}
