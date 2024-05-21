using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSlam : Skill
{
    private Coroutine drawswordCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(3, 3, 3);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/DrawSword.png");
    }

    public override IEnumerator StartSkillCast()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        _animator.CrossFade("SHIELD_SLAM", 0.05f);
        yield return new WaitForSeconds(0.3f);
        Managers.Sound.Play("Skill/ShieldSlam");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage * 2);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 2;
        hitbox.transform.localScale = skillRange;

        KnockBackBox knockBackBox = Managers.Resource.Instantiate("Skill/KnockBackBoxRect").GetComponent<KnockBackBox>();
        knockBackBox.SetUp(transform, 13, 0.3f);
        //knockBackBox.transform.parent = hitbox.transform;
        knockBackBox.transform.position = gameObject.transform.position + transform.forward * 2;
        knockBackBox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ShieldSlam, 0.0f, transform);

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(0.4f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }


}
