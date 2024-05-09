using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlWind : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 25;
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(5, 5, 5);
        CollavoSkillRange = new Vector3(20, 5, 10);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Warrior/Whirlwind.png");
        CollavoSkillName = "ThunderStorm";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("WHIRLWIND", 0.1f);
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.WhirlwindEffect1, gameObject.transform);
        ParticleSystem ps2 = Managers.Effect.Play(Define.Effect.WhirlwindEffect2, gameObject.transform);
        for (int i = 0; i < 8; i++)
        {
            Managers.Sound.Play("Skill/Whirlwind");
            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = gameObject.transform.position;
            hitbox.transform.localScale = skillRange;
            yield return new WaitForSeconds(0.2f);
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.1f);
        Managers.Effect.Stop(ps1);
        Managers.Effect.Stop(ps2);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("WHIRLWIND", 0.1f);
        ParticleSystem ps1 = Managers.Effect.Play(Define.Effect.WhirlwindEffect1, gameObject.transform);

        for (int i = 0; i < 5; i++)
        {
            Vector3 randomPos = gameObject.transform.position + new Vector3
                                (Random.Range(-CollavoSkillRange.x / 8, CollavoSkillRange.x * 7 / 8),
                                0,
                                Random.Range(-CollavoSkillRange.z / 2, CollavoSkillRange.z / 2));

            HitBox hiddenbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hiddenbox.transform.position = randomPos;

            yield return new WaitForSeconds(0.2f);
            StartCoroutine(TornadoCoroutine(Define.Effect.WhirlwindEffect1, hiddenbox.transform));
        }

        yield return new WaitForSeconds(0.2f);
        Managers.Effect.Stop(ps1);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator TornadoCoroutine(Define.Effect effect, Transform hitbox)
    {
        ParticleSystem ps = Managers.Effect.Play(effect, hitbox.transform);
        ps.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

        yield return new WaitForSeconds(3.0f);
        Managers.Effect.Stop(ps);
    }
}
