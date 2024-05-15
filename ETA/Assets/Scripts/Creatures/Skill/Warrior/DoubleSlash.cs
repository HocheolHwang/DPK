using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSlash : Skill
{
    private Coroutine drawswordCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(2, 3, 2);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/DrawSword.png");
    }

    public override IEnumerator StartSkillCast()
    {

        _controller.SkillSlot.PreviousSkill = this;
        yield return new WaitForSeconds(0.05f);
        Managers.Sound.Play("Skill/RSkill");
        _animator.CrossFade("ATTACK1", 0.05f);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.DoubleSlash1, 0.0f, transform);
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.15f);
        Managers.Resource.Destroy(hitbox.gameObject);
        _animator.CrossFade("ATTACK2", 0.15f);
        ps = Managers.Effect.Play(Define.Effect.DoubleSlash2, 0.0f, transform);
        Managers.Sound.Play("Skill/RSkill");
        hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;
        yield return new WaitForSeconds(0.05f);
        Managers.Resource.Destroy(hitbox.gameObject);
        
        ChangeToPlayerMoveState();
    }


}
