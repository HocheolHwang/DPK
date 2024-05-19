using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plague : Skill
{
    private Vector3 targetPos;

    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 1;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(5, 5, 5);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/Plague.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);
        targetPos = _skillSystem.TargetPosition;

        yield return new WaitForSeconds(0.1f);
        if (_controller.Stat.Hp <= Damage)
            StartCoroutine(LowHpCoroutine());
        else
            StartCoroutine(PlagueCoroutine());

        yield return new WaitForSeconds(0.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator PlagueCoroutine()
    {
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage;
        Managers.Sound.Play("Skill/Plague");
        _controller.DecreaseHp(Damage);
        if (_controller.Stat.Hp < 0) _controller.Stat.Hp = 0;

        ParticleSystem ps = Managers.Effect.Play(Define.Effect.Plague, 2.5f, transform);
        ps.transform.position = targetPos + new Vector3(0, 1, 0);
        ps.transform.localScale = skillRange;

        BuffBox buffbox = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox.SetUp(transform, -(Damage / 10), BuffBox.stat.Defense, 5.0f, "Monster");
        buffbox.transform.position = targetPos;
        buffbox.transform.localScale = skillRange;

        yield return new WaitForSeconds(0.25f);
        Managers.Resource.Destroy(buffbox.gameObject);
    }

    private IEnumerator LowHpCoroutine()
    {
        Managers.Sound.Play("Skill/LowHp");
        yield return new WaitForSeconds(0.25f);
    }
}
