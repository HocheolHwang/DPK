using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmediatelySkill : TmpSkill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 30;
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        base.Init();

    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("TEMP", 0.1f);
        yield return new WaitForSeconds(0.1f);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBox").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
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
