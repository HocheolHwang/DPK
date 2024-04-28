using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSkill : TmpSkill
{
    protected override void Init()
    {
        _cooldownTime = 4;
        base.Init();
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBox").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = _skillSystem.TargetPosition;
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
