using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Skill
{
    private Coroutine healCoroutine;

    protected override void Init()
    {
        SetCoolDownTime(15);
        SkillType = Define.SkillType.Immediately;
        base.Init();
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/Heal.png");
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("CASTING_IN", 0.1f);
        Managers.Sound.Play("Skill/Heal");

        yield return new WaitForSeconds(0.2f);
        _animator.CrossFade("CASTING_WAIT", 0.1f);
        Managers.Coroutine.Run(HealCoroutine());

        yield return new WaitForSeconds(0.5f);
        _animator.CrossFade("CASTING_OUT", 0.1f);

        yield return new WaitForSeconds(0.3f);
        //_controller.ChangeState(_controller.MOVE_STATE);
        ChangeToPlayerMoveState();
    }


    private IEnumerator HealCoroutine()
    {
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.EnergyNovaGreen, 2.0f, gameObject.transform);
        Damage = _controller.GetComponent<PlayerStat>().AttackDamage / 10;

        for (int i = 0; i < 10; i++)
        {
            BuffBox buffbox = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
            buffbox.SetUp(transform, Damage, BuffBox.stat.Hp);
            buffbox.transform.position = gameObject.transform.position;
            buffbox.transform.localScale = new Vector3(20, 3, 20);
            yield return new WaitForSeconds(0.1f);
            Managers.Resource.Destroy(buffbox.gameObject);
        }
    }
}
