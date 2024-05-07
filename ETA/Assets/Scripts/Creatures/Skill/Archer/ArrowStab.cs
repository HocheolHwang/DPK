using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowStab : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        //skillIcon.sprite = Resources.Load<Sprite>("Sprites/SkillIcon/Archer/ArrowStab.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("ATTACK5", 0.1f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/ArrowStab");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, false);
        //hitbox.transform.position = gameObject.transform.position + transform.forward;
        hitbox.transform.position = _skillSystem.TargetPosition - transform.forward ;
        hitbox.transform.localScale = skillRange;

        //ParticleSystem ps = Managers.Resource.Instantiate("Effect/ArrowStab").GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(0.2f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ArrowStab, gameObject.transform);
        //ps.transform.position = gameObject.transform.position + transform.forward;

        ps.Play();
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(1.0f);
        //Managers.Resource.Destroy(ps.gameObject);
        Managers.Effect.Stop(ps);
        _controller.ChangeState(_controller.MOVE_STATE);
    }
}
