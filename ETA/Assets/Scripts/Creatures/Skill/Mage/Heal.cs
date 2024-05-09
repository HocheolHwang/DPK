using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    private Coroutine protectionCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(1);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Heal.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("CASTING_IN", 0.1f);
        Managers.Sound.Play("Skill/Heal");
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.EnergyNovaGreen, _skillSystem.transform);

        yield return new WaitForSeconds(0.2f);
        _animator.CrossFade("CASTING_WAIT", 0.1f);
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.1f);
            protectionCoroutine = StartCoroutine(ProtectionCoroutine());
        }

        yield return new WaitForSeconds(0.1f);
        if (ps != null)
            Managers.Effect.Stop(ps);
        _animator.CrossFade("CASTING_OUT", 0.1f);
        yield return new WaitForSeconds(0.3f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }


    private IEnumerator ProtectionCoroutine()
    {
        BuffBox buffbox = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox.SetUp(transform, 1, BuffBox.stat.Hp);
        buffbox.transform.position = gameObject.transform.position + transform.forward;
        buffbox.transform.localScale = new Vector3(20, 3, 20);
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.AuraChargeGreen, buffbox.transform);
        yield return new WaitForSeconds(0.4f);
        Managers.Resource.Destroy(buffbox.gameObject);
        if (ps != null)
            Managers.Effect.Stop(ps);
    }
}
