using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSword : Skill
{
    private Coroutine drawswordCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(5);
        Damage = 20;
        base.Init();
        SkillType = Define.SkillType.Immediately;
        skillRange = new Vector3(4, 3, 4);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/DrawSword.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("TEMP", 0.1f);
        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/RSkill");
        HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
        hitbox.SetUp(transform, Damage, -1, true);
        hitbox.transform.position = gameObject.transform.position + transform.forward * 1.5f;
        hitbox.transform.localScale = skillRange;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.SlashWideBlue, 0.0f, transform);

        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(hitbox.gameObject);
        yield return new WaitForSeconds(0.05f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }


}
