using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whirlwind : TmpSkill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(5, 5, 5);
    }

    public override IEnumerator StartSkillCast()
    {


        ParticleSystem ps = Managers.Resource.Instantiate("Effect/BasicTornado").GetComponent<ParticleSystem>();
        ps.transform.position = gameObject.transform.position + transform.up;

        ps.Play();
        _animator.CrossFade("WHIRLWIND", 0.1f);


        for (int i = 0; i < 8; i++)
        {
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = gameObject.transform.position;
            hitbox.transform.localScale = skillRange;
            yield return new WaitForSeconds(0.15f);
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(ps.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
