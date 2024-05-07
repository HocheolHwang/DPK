using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protection : Skill
{
    private Coroutine protectionCoroutine;
    private ParticleSystem ps;
    protected override void Init()
    {
        SetCoolDownTime(1);
        SkillType = Define.SkillType.Immediately;
        base.Init();
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL5", 0.1f);
        Managers.Sound.Play("Skill/Heal");
        ps = Managers.Effect.Play(Define.Effect.AuraChargeBlue, gameObject.transform);

        // 파티클 시스템을 캐릭터의 자식으로 설정
        if (ps != null)
            ps.transform.SetParent(gameObject.transform);

        protectionCoroutine = StartCoroutine(ProtectionCoroutine());

        yield return new WaitForSeconds(1.0f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }


    private IEnumerator ProtectionCoroutine()
    {
        BuffBox buffbox = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox.SetUp(transform, 100);
        buffbox.transform.position = gameObject.transform.position;
        Debug.Log($"buffbox position: {buffbox.transform.position}");
        buffbox.transform.localScale = new Vector3(10, 5, 10);

        yield return new WaitForSeconds(5.0f);
        Debug.Log($"Defense: {_controller.Stat.Shield}");
        if (ps != null)
            Managers.Effect.Stop(ps);
        if (_controller.Stat.Shield != 0)
            _controller.RemoveShield(100);
    }
}
