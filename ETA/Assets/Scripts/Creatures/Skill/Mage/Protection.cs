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
        BuffBox buffbox1 = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox1.SetUp(transform, 100, BuffBox.stat.Shield);
        buffbox1.transform.position = gameObject.transform.position;
        buffbox1.transform.localScale = new Vector3(10, 5, 10);
        yield return new WaitForSeconds(0.1f);
        Managers.Resource.Destroy(buffbox1.gameObject);

        yield return new WaitForSeconds(5.0f);
        Debug.Log($"Defense: {_controller.Stat.Shield}");
        BuffBox buffbox2 = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox2.SetUp(transform, -100, BuffBox.stat.Shield);
        buffbox2.transform.position = gameObject.transform.position;
        buffbox2.transform.localScale = new Vector3(10, 5, 10);
        if (ps != null)
            Managers.Effect.Stop(ps);
    }
}
