using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blessing : Skill
{
    private Coroutine blessingCoroutine;
    private ParticleSystem ps;

    protected override void Init()
    {
        SetCoolDownTime(15);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Blessing.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("BUFF2", 0.1f);
        Managers.Sound.Play("Skill/Heal");
        ps = Managers.Effect.Play(Define.Effect.BlessingEffect, gameObject.transform);
        _controller.Stat.Defense += 10;
        Debug.Log($"Defense: {_controller.Stat.Defense}");

        // 파티클 시스템을 캐릭터의 자식으로 설정
        if (ps != null)
            ps.transform.SetParent(gameObject.transform);

        // 방어력 증가를 시작하고, 일정 시간 후에 감소시키는 코루틴을 시작합니다.
        blessingCoroutine = StartCoroutine(BlessingCoroutine());

        yield return new WaitForSeconds(0.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator BlessingCoroutine()
    {
        // 방어력 증가 후 일정 시간 대기
        yield return new WaitForSeconds(5.0f);

        // 방어력 감소 및 파티클 중지
        _controller.Stat.Defense -= 10;
        Debug.Log($"Defense: {_controller.Stat.Defense}");
        if (ps != null)
            Managers.Effect.Stop(ps);
    }
}
