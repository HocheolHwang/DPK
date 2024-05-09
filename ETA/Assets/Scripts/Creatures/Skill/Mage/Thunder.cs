using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : Skill
{
    protected override void Init()
    {
        SetCoolDownTime(1);
        Damage = 10;
        base.Init();
        SkillType = Define.SkillType.Holding;
        skillRange = new Vector3(1, 5, 1);
        CollavoSkillRange = new Vector3(20, 5, 10);
        skillIcon = Resources.Load<Sprite>("Sprites/SkillIcon/Mage/Thunder.png");
        CollavoSkillName = "ThunderStorm";
    }

    public override IEnumerator StartSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);
        for (int i = 0; i < 4; i++)
        {
            Define.Effect effectName1 = (i % 2 == 0) ? Define.Effect.Thunder1 : Define.Effect.Thunder2;
            Define.Effect effectName2 = (i % 2 != 0) ? Define.Effect.Thunder1 : Define.Effect.Thunder2;
            Managers.Sound.Play("Skill/Thunder");

            HitBox hitbox1 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox1.SetUp(transform, Damage);
            hitbox1.transform.position = gameObject.transform.position + transform.forward * (i + 1);
            hitbox1.transform.localScale = skillRange;
            StartCoroutine(ThunderCoroutine(effectName1, hitbox1.transform));

            HitBox hitbox2 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox2.SetUp(transform, Damage);
            hitbox2.transform.position = gameObject.transform.position + transform.forward * (i + 1) + transform.right * i;
            hitbox2.transform.localScale = skillRange;
            StartCoroutine(ThunderCoroutine(effectName2, hitbox2.transform));

            HitBox hitbox3 = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox3.SetUp(transform, Damage);
            hitbox3.transform.position = gameObject.transform.position + transform.forward * (i + 1) + transform.right * -i;
            hitbox2.transform.localScale = skillRange;
            StartCoroutine(ThunderCoroutine(effectName2, hitbox3.transform));

            yield return new WaitForSeconds(0.15f);
            Managers.Resource.Destroy(hitbox1.gameObject);
            Managers.Resource.Destroy(hitbox2.gameObject);
            Managers.Resource.Destroy(hitbox3.gameObject);
        }

        yield return new WaitForSeconds(0.1f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    public override IEnumerator StartCollavoSkillCast()
    {
        _animator.CrossFade("SKILL1", 0.1f);
        Managers.Sound.Play("Skill/ThunderStorm");

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPos = gameObject.transform.position + new Vector3
                                (Random.Range(-CollavoSkillRange.x / 8, CollavoSkillRange.x * 7 / 8),
                                0,
                                Random.Range(-CollavoSkillRange.z / 2, CollavoSkillRange.z / 2));

            HitBox hitbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hitbox.SetUp(transform, Damage);
            hitbox.transform.position = gameObject.transform.position + transform.forward * 5;
            hitbox.transform.localScale = CollavoSkillRange;

            HitBox hiddenbox = Managers.Resource.Instantiate("Skill/HitBoxRect").GetComponent<HitBox>();
            hiddenbox.transform.position = randomPos;

            Define.Effect effectName = (i % 2 == 0) ? Define.Effect.Thunder1 : Define.Effect.Thunder2;
            StartCoroutine(ThunderCoroutine(effectName, hiddenbox.transform));
            Managers.Sound.Play("Skill/Thunder");

            yield return new WaitForSeconds(0.1f); // 번개 이펙트 간격 조절
            Managers.Resource.Destroy(hitbox.gameObject);
        }

        yield return new WaitForSeconds(0.2f);
        _controller.ChangeState(_controller.MOVE_STATE);
    }

    private IEnumerator ThunderCoroutine(Define.Effect effect, Transform hitbox)
    {
        ParticleSystem ps = Managers.Effect.Play(effect, hitbox.transform);

        yield return new WaitForSeconds(2.0f);
        Managers.Effect.Stop(ps);
    }
}
