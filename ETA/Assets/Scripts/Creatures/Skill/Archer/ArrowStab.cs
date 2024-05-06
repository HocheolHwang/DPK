using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStab : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(2);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Target;
        skillRange = new Vector3(3, 3, 1);
        skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ArrowStab.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("ATTACK4", 0.1f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/ArrowStab");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowStab").GetComponent<ParticleSystem>();
        ps.transform.position = gameObject.transform.position + transform.forward;

        ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.0f);
        Managers.Resource.Destroy(ps.gameObject);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
