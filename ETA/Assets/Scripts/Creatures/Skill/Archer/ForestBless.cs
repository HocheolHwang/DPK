using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class ForestBless : Skill
{
    private Coroutine forestBlessCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(20);
        SkillType = Define.SkillType.Immediately;
        base.Init();
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL7", 0.1f);
        yield return new WaitForSeconds(0.5f);
        Managers.Sound.Play("Skill/ForestBless");
        Managers.Coroutine.Run(ForestBlessCoroutine());
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(0.1f);
        ChangeToPlayerMoveState();
    }

    private IEnumerator ForestBlessCoroutine()
    {
        ParticleSystem ps00 = Managers.Effect.Play(Define.Effect.ForestBless, 1.0f, gameObject.transform);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.ForestBlessAura, 10.0f, gameObject.transform);
        ps.transform.position = transform.position - transform.up*0.2f;
        if (ps != null)
            ps.transform.SetParent(gameObject.transform);

        _controller.IncreaseDamage(10);

        // 공격력 증가 후 일정 시간 대기
        yield return new WaitForSeconds(10.0f);
        _controller.DecreaseDamage(10); 
    }
}
