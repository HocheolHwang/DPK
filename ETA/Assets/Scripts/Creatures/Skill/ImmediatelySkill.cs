using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediatelySkill : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 10;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("TEMP", 0.1f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Resource.Instantiate("Effect/SlashWideBlue").GetComponent<ParticleSystem>();
        ps.transform.position = gameObject.transform.position + transform.up;
        
        ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.0f);
        Managers.Resource.Destroy(ps.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
