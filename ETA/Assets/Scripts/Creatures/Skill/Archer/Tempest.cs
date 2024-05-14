using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tempest : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(8);
        Damage = 10;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/DrawSword.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("ATTACK5", 0.05f);
        yield return new WaitForSeconds(0.3f);
        Managers.Sound.Play("Skill/ShieldSlam");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;

        KnockBackBox knockBackBox = Managers.Resource.Instantiate("Skill/KnockBackBoxRect").GetComponent<KnockBackBox>();
        knockBackBox.SetUp(transform, 12, 0.5f);
        //knockBackBox.transform.parent = hitbox.transform;
        knockBackBox.transform.position = gameObject.transform.position + transform.forward * 2;
        knockBackBox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Tempest, 0.0f, transform);
        ps.transform.position = transform.position + transform.forward;

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(0.4f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }
}
