using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonMist : Skill
{
    private Vector3 targetPos;

    protected override void Init()
    {
        SetCoolDownTime(15);
        Damage = 1;
        base.Init();
        SkillType = Define.SkillType.Range;
        skillRange = new Vector3(5, 5, 5);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/PoisonMist.png");
    }
    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL2", 0.1f);
        targetPos = _skillSystem.TargetPosition;

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(PoisonMistCoroutine());

        yield return new WaitForSeconds(0.5f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator PoisonMistCoroutine()
    {
        Managers.Sound.Play("Skill/PoisonMist");
        ParticleSystem ps = Managers.Effect.Play(Define.Effect.PoisonMist, 5.0f, transform);
        ps.transform.position = targetPos + new Vector3(0, 0.5f, 0);

        BuffBox buffbox = Managers.Resource.Instantiate("Skill/BuffBoxRect").GetComponent<BuffBox>();
        buffbox.SetUp(transform, -1, BuffBox.stat.MoveSpeed, 5.0f, "Monster");
        buffbox.transform.position = targetPos;
        buffbox.transform.localScale = skillRange;

        for (int i = 0; i < 20; i++)
        {
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = targetPos;
            hitbox.transform.localScale = skillRange;

            yield return new WaitForSeconds(0.25f);
            Managers.Resource.Destroy(hitbox.gameObject);
            Managers.Resource.Destroy(buffbox.gameObject);
        }

    }
}
