using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickFreeze : Skill
{

    protected override void Init()
    {
        SetCoolDownTime(15);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/QuickFreeze.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.speed = 0;

        Managers.Coroutine.Run(QuickFreezeCoroutine());
        Managers.Coroutine.Run(DefenseUp(2.0f));

        yield return new WaitForSeconds(0.1f);
        Managers.Sound.Play("Skill/QuickFreezeIn");

        yield return new WaitForSeconds(1.5f);
        _animator.speed = 1.0f;
        Managers.Sound.Play("Skill/QuickFreezeOut");
        ChangeToPlayerMoveState();
    }

    IEnumerator DefenseUp(float duration)
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        _controller.Stat.Defense += Damage;
        yield return new WaitForSeconds(duration);
        _controller.Stat.Defense -= Damage;
    }

    IEnumerator QuickFreezeCoroutine()
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.QuickFreeze, 2.0f, gameObject.transform);
        ps.transform.position -= new Vector3(0, 1, 0);
        ps.transform.localScale = new Vector3(1.25f, 1.25f, 1.25f);
        yield return new WaitForSeconds(0.1f);
    }
}
