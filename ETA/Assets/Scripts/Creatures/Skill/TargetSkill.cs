using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : TmpSkill
{
    protected override void Init()
    {
        SetCoolDownTime(4);
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
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBox").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);

        yield return new WaitForSeconds(1.5f);
        Managers.Resource.Destroy(ps1.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
